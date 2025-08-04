using UnityEngine;

public class SnakeFireProjectile : BaseProjectile
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.6f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Shoot()
    {
        Shoot(transform.position, Vector2.left);
    }

    public void Shoot(Vector2 position, Vector2 direction)
    {
        Vector2 offset = direction.normalized * spawnOffset + Vector2.down * 0.2f;
        transform.position = position + offset; // Both are Vector2
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * _speed;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
        transform.localScale = scale;

        CancelInvoke(nameof(DisableSelf));
        Invoke(nameof(DisableSelf), lifetime);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

protected override void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player"))
    {
        Debug.Log($"[SnakeFireProjectile] Ignored collision with: {other.name}");
        return;
    }

    if (other.TryGetComponent<IDamageable>(out var damageable))
    {
        Debug.Log($"[SnakeFireProjectile] Hitting player: {other.name} for {damage} damage");
        damageable.TakeDamage(damage);
        DisableSelf();
    }
    else
    {
        Debug.Log($"[SnakeFireProjectile] Player has no IDamageable: {other.name}");
    }
}


    protected override void OnDisable()
    {
        CancelInvoke();
        if (rb != null) rb.velocity = Vector2.zero;
    }
}
