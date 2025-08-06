using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class AxeWeapon : BaseWeapon, IAttacker
{
    private ProjectileAxePool _axePool;

    [Inject]
    private void Awake()
    {
        Debug.Log($"[AxeWeapon] Awake! Pool is {(_axePool == null ? "null" : "set")}");
    }

    [Inject]
    public void Construct(ProjectileAxePool axePool)
    {
        Debug.Log("[AxeWeapon] Construct called!");
        _axePool = axePool;
    }

    protected override void Fire()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning("[AxeWeapon] ❌ No parent — cannot determine direction.");
            return;
        }

        Vector3 spawnPos = transform.parent.position;
        float direction = Mathf.Sign(transform.parent.localScale.x);

        Vector2 parentVelocity = transform.parent.TryGetComponent(out Rigidbody2D rb)
            ? rb.velocity
            : Vector2.zero;

        if (_axePool == null)
        {
            Debug.LogError("[AxeWeapon] ❌ Axe pool is null.");
            return;
        }

        var axe = _axePool.Get(spawnPos, Quaternion.identity);

        if (axe == null)
        {
            Debug.LogError("[AxeWeapon] ❌ Pool.Get() returned null.");
            return;
        }

        axe.SetAttacker(this);
        axe.Shoot(spawnPos, Vector2.right * direction, parentVelocity.magnitude);
    }
    public bool PoolIsSet() => _axePool != null;

}
