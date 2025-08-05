using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public sealed class ProjectileSpark : BaseProjectile
{
    [SerializeField] private float lifetime = 0.3f;
    [SerializeField] private float flickerInterval = 0.05f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _flip;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        transform.position = origin;
        _rb.velocity = dir.normalized * _speed;

        _flip = dir.x < 0;
        _sr.flipX = _flip;

        InvokeRepeating(nameof(ToggleFlip), 0f, flickerInterval);
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void ToggleFlip()
    {
        _sr.flipX = _flip = !_flip;
    }

    private void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }

    public override void ResetState()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
        _sr.flipX = false;
    }
}
