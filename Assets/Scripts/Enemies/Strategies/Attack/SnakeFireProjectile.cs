using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class SnakeFireProjectile : BaseProjectile
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.6f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        transform.position = origin + dir.normalized * spawnOffset + Vector2.down * 0.2f;
        _rb.velocity = dir.normalized * _speed;

        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        base.OnTriggerEnter2D(other);
    }

    public override void ResetState()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
    }
}
