using UnityEngine;

public class ProjectileSpark : BaseProjectile
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 0.3f;
    [SerializeField] private float sparkSpeed = 5f;
    [SerializeField] private float flickerInterval = 0.05f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool flickerState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Shoot()
    {
        Shoot(transform.position, Vector2.right);
    }

    public void Shoot(Vector2 position, Vector2 direction)
    {
        Shoot(position, direction, 0f);
    }

    public void Shoot(Vector2 position, Vector2 direction, float playerSpeed)
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        float additionalSpeed = Mathf.Max(0f, Vector2.Dot(direction.normalized, new Vector2(playerSpeed, 0f)));
        rb.velocity = direction.normalized * (sparkSpeed + additionalSpeed);

        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction.x),
            transform.localScale.y,
            transform.localScale.z
        );

        flickerState = direction.x < 0;
        spriteRenderer.flipX = flickerState;

        InvokeRepeating(nameof(FlickerFlip), 0f, flickerInterval);
        Invoke(nameof(DisableSelf), lifetime);

        Debug.Log($"[ProjectileSpark] Shoot → direction: {direction}, velocity: {rb.velocity}, position: {position}");
    }

    private void FlickerFlip()
    {
        flickerState = !flickerState;
        if (spriteRenderer != null)
            spriteRenderer.flipX = flickerState;
    }

    private void DisableSelf()
    {
        Debug.Log("[ProjectileSpark] Disabled");
        CancelInvoke();
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[ProjectileSpark] Triggered by: {other.name}");

        if (other.TryGetComponent(out IObstacle obstacle))
        {
            Debug.Log($"[ProjectileSpark] Hit IObstacle: {obstacle.Type}");

            if (CanDestroy(obstacle.Type))
            {
                Debug.Log("[ProjectileSpark] Obstacle is destructible → Destroying");
                obstacle.DestroyObstacle();
                DisableSelf();
                return;
            }

            Debug.Log("[ProjectileSpark] Obstacle is NOT destructible");
        }

        if (other.TryGetComponent(out IDamageable dmg))
        {
            Debug.Log("[ProjectileSpark] Hit IDamageable → Applying damage");
            dmg.TakeDamage(damage);
            DisableSelf();
        }
    }

    protected virtual bool CanDestroy(ObstacleType type)
    {
        return type == ObstacleType.Rock;
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        if (rb != null) rb.velocity = Vector2.zero;
    }
}
