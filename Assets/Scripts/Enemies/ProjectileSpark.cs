using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public sealed class ProjectileSpark : BaseProjectile
{
    [Header("Spark Settings")]
    [SerializeField] private float lifetime = 0.3f;
    [SerializeField] private float flickerInterval = 0.05f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _flipState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public override void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;

        _rb.velocity = direction.normalized * _speed;
        _flipState = direction.x < 0;
        _sr.flipX = _flipState;

        InvokeRepeating(nameof(ToggleFlip), 0f, flickerInterval);
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void ToggleFlip()
    {
        _flipState = !_flipState;
        _sr.flipX = _flipState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other);
        ReturnToPool();
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
        _sr.flipX = false;
        _flipState = false;
    }
}
