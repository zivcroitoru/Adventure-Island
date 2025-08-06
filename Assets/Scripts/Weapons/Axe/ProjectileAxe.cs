using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public sealed class ProjectileAxe : BaseProjectile
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float horizontalSpeed = 7f;
    [SerializeField] private float verticalBoost = 4f;
    [SerializeField] private float spinSpeed = 720f; // degrees per second

    private Rigidbody2D _rb;
    private float _spinDirection = 1f;
    private bool _spinning = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true; // manual rotation, so freeze physics
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float _ = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;

        float xDir = Mathf.Sign(dir.x);
        Vector2 velocity = new Vector2(xDir * horizontalSpeed, verticalBoost);

        _rb.gravityScale = 1f;
        _rb.velocity = velocity;

        _spinDirection = -xDir; // flip spin based on facing
        _spinning = true;

        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void Update()
    {
        if (_spinning)
        {
            float rotationAmount = spinSpeed * Time.deltaTime * _spinDirection;
            transform.Rotate(0f, 0f, rotationAmount);
        }
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _spinning = false;
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }
}
