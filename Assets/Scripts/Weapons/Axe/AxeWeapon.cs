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
    var parentTf = transform.parent;
    if (!parentTf || _axePool == null) return;

    Vector3 spawnPos = parentTf.position;
    float dirSign = Mathf.Sign(parentTf.localScale.x);
    Vector2 dir = new Vector2(dirSign, 0f);

    Vector2 parentVelocity = parentTf.TryGetComponent<Rigidbody2D>(out var rb)
        ? rb.velocity
        : Vector2.zero;

    var axe = _axePool.Get(spawnPos, Quaternion.identity);
    if (!axe) return;

    // Always the player → just set owner to the player's root
    axe.SetOwner(parentTf.root);

    const float fallbackSpeed = 8f;
    float launchSpeed = parentVelocity.magnitude > 0.05f ? parentVelocity.magnitude : fallbackSpeed;

    axe.Shoot(spawnPos, dir, launchSpeed);
}


    public bool PoolIsSet() => _axePool != null;
}
