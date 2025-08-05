using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class ProjectileFire : BaseProjectile
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float spawnOffset = 0.3f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        Vector2 offset = dir.normalized * spawnOffset + Vector2.down * 0.2f;
        transform.position = origin + offset;
        transform.rotation = Quaternion.identity;

        float extra = Mathf.Max(0f, Vector2.Dot(dir.normalized, new Vector2(playerSpeed, 0)));
        _rb.velocity = dir.normalized * (_speed + extra);

        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }

    public override void ResetState()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
    }
}
