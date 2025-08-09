using UnityEngine;

public sealed class ProjectileSpark : BaseProjectile
{
    [SerializeField] private float lifetime        = 0.3f;
    [SerializeField] private float flickerInterval = 0.05f;

    bool _flipState;

    public override void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f)
    {
        var n = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;

        transform.rotation = Quaternion.identity;
        base.Shoot(origin, n, _speed);       // sets position + Rb.velocity

        _flipState = n.x < 0f;
        if (Sr) Sr.flipX = _flipState;

        InvokeRepeating(nameof(ToggleFlip), 0f, flickerInterval);
        Invoke(nameof(ReturnToPool), lifetime);
    }

    void ToggleFlip()
    {
        _flipState = !_flipState;
        if (Sr) Sr.flipX = _flipState;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();                   // zeros Rb.velocity
        CancelInvoke(nameof(ToggleFlip));   // stop flicker
        CancelInvoke();                     // stop lifetime if any
        _flipState = false;
        if (Sr) Sr.flipX = false;          // reset for pooled reuse
    }
}
