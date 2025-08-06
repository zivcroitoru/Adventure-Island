using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class ProjectileFire : BaseProjectile
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.3f;
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer spriteRenderer; // assign in prefab

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Do NOT destroy obstacles directly!
        // DamageDealer on the projectile (or wherever) should handle it centrally.

        ReturnToPool();
    }


public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
{
    Vector2 offset = dir.normalized * spawnOffset + Vector2.down * 0.2f;
    transform.position = origin + offset;
    transform.rotation = Quaternion.identity;

    float bonus = Mathf.Max(0f, Vector2.Dot(dir.normalized, new Vector2(playerSpeed, 0f)));
    _rb.velocity = dir.normalized * (_speed + bonus);

    // 💡 Flip sprite based on direction
    if (spriteRenderer != null)
        spriteRenderer.flipX = dir.x < 0f;

    Invoke(nameof(ReturnToPool), lifetime);
}


    public override void OnDespawn()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
    }
}
