using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageDealer))]
public sealed class SnakeFireProjectile : BaseProjectile, IObstacle
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.6f;

    // IObstacle
    public ObstacleType Type => ObstacleType.Projectile;
    public int ContactDamage => 999;
    public int RidingDamage  => 999;
    public void DestroyObstacle() => ReturnToPool();

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        Vector2 offset = dir.normalized * spawnOffset + Vector2.down * 0.2f;
        base.Shoot(origin + offset, dir, _speed);   // positions + sets Rb.velocity
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();   // zeroes Rb.velocity in base
        CancelInvoke();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        base.OnTriggerEnter2D(other);
    }
}
