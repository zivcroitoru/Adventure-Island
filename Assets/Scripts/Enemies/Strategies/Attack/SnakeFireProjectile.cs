using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageDealer))]
public sealed class SnakeFireProjectile : BaseProjectile, IObstacle
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.6f;

    private Rigidbody2D _rb;

    // --- IObstacle ---
    public ObstacleType Type => ObstacleType.Projectile;
    public int ContactDamage => 1;
    public int RidingDamage  => 0;

    public void DestroyObstacle() => ReturnToPool();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        Vector2 offset = dir.normalized * spawnOffset + Vector2.down * 0.2f;
        transform.position = origin + offset;
        _rb.velocity = dir.normalized * _speed;

        Invoke(nameof(ReturnToPool), lifetime);
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        base.OnTriggerEnter2D(other);
        // Optional: only if BaseProjectile does something useful on hit
    }
}
