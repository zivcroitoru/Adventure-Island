using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class AxeWeapon : BaseWeapon
{
    private ProjectileAxePool _axePool;

    [Inject]
    public void Construct(ProjectileAxePool axePool)
    {
        _axePool = axePool;
    }

    protected override void Fire()
    {
        if (transform.parent == null)
            return;

        Vector3 spawnPos = transform.parent.position;
        float direction = Mathf.Sign(transform.parent.localScale.x);

        Vector2 parentVelocity = transform.parent.TryGetComponent(out Rigidbody2D rb)
            ? rb.velocity
            : Vector2.zero;

        if (_axePool == null)
            return;

        var axe = _axePool.Get(spawnPos, Quaternion.identity);
        if (axe == null)
            return;

        axe.SetAttacker(this);
        axe.Shoot(spawnPos, Vector2.right * direction, parentVelocity.magnitude);
    }

    public bool PoolIsSet() => _axePool != null;
}
