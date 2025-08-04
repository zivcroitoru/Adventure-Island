using UnityEngine;

public class ProjectileFire : BaseProjectile
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        direction *= -1f;

        transform.position = position;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        float additionalSpeed = Mathf.Max(0, Vector2.Dot(direction.normalized, new Vector2(playerSpeed, 0)));
        rb.velocity = direction.normalized * (_speed + additionalSpeed);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
        transform.localScale = scale;

        CancelInvoke(nameof(DisableSelf));
        Invoke(nameof(DisableSelf), lifetime);

        Debug.Log($"[ProjectileFire] Shoot → direction: {direction}, speed: {_speed + additionalSpeed}, position: {position}");
    }

    private void DisableSelf()
    {
        Debug.Log("[ProjectileFire] Disabled");
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[ProjectileFire] Triggered by: {other.name}");

        if (other.TryGetComponent(out IObstacle obstacle))
        {
            Debug.Log($"[ProjectileFire] Hit IObstacle: {obstacle.Type}");

            if (CanDestroy(obstacle.Type))
            {
                Debug.Log("[ProjectileFire] Obstacle is destructible → Destroying");
                obstacle.DestroyObstacle();
                DisableSelf();
                return;
            }
            else
            {
                Debug.Log("[ProjectileFire] Obstacle is NOT destructible");
            }
        }

        if (other.TryGetComponent(out IDamageable dmg))
        {
            Debug.Log("[ProjectileFire] Hit IDamageable → Applying damage");
            dmg.TakeDamage(damage);
            DisableSelf();
        }
    }

    protected virtual bool CanDestroy(ObstacleType type)
    {
        // Default logic: can destroy everything except Fire
        return type != ObstacleType.Fire;
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        if (rb != null) rb.velocity = Vector2.zero;
    }
}
