using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public sealed class ProjectileAxe : BaseProjectile
{
    [Header("Axe Motion")]
    [SerializeField] float lifetime        = 3f;
    [SerializeField] float horizontalSpeed = 7f;
    [SerializeField] float verticalBoost   = 4f;
    [SerializeField] float spinSpeed       = 720f; // deg/sec

    float _spinDirection = 1f;
    bool  _spinning = false;

    void Awake()
    {
        Rb.freezeRotation = true;
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    public override void Shoot(Vector2 origin, Vector2 dir, float _ = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;

        float xDir = Mathf.Sign(dir.x == 0f ? 1f : dir.x);
        Rb.gravityScale = 1f;
        Rb.velocity     = new Vector2(xDir * horizontalSpeed, verticalBoost);

        _spinDirection  = -xDir;
        _spinning       = true;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    void Update()
    {
        if (_spinning)
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime * _spinDirection);
    }

    // ✅ match BaseProjectile's new signature
    protected override void HandleHit(GameObject hitGO)
    {
        base.HandleHit(hitGO); // routes through Damage.Deal(...)
        ReturnToPool();        // axes despawn on first valid hit
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        base.OnDespawn();
        Rb.gravityScale = 0f;
        _spinning = false;
        transform.rotation = Quaternion.identity;
    }
}
