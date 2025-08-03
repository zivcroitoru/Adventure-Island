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

    // Required by BaseProjectile abstract class
    public override void Shoot()
    {
        // Fallback: shoot from current position to the right
        Shoot(transform.position, Vector2.right);
    }

    // Called externally with custom position and direction
public void Shoot(Vector2 position, Vector2 direction)
{
    transform.position = position;
    transform.rotation = Quaternion.identity;
    gameObject.SetActive(true);

    if (rb == null) rb = GetComponent<Rigidbody2D>();
    rb.velocity = direction.normalized * _speed *-1f;

    // 🔁 Flip sprite if needed
    Vector3 scale = transform.localScale;
    scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
    transform.localScale = scale;

    CancelInvoke(nameof(DisableSelf));
    Invoke(nameof(DisableSelf), lifetime);

    Debug.Log($"[ProjectileFire] Shoot at {direction}");
}


    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(damage);
        }

        DisableSelf();
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}
