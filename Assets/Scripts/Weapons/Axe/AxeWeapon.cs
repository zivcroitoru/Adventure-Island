using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class AxeWeapon : BaseWeapon, IAttacker
{
    [Inject] private PoolManager _poolManager;

    [SerializeField] private float _projectileSpeed = 5f;

    protected override void Fire()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning("[AxeWeapon] No parent — cannot determine direction.");
            return;
        }

        Vector3 spawnPos = transform.parent.position;
        float direction = Mathf.Sign(transform.parent.localScale.x);

        Vector2 velocity = transform.parent.TryGetComponent(out Rigidbody2D rb)
            ? rb.velocity
            : Vector2.zero;

        var axe = _poolManager.SpawnAxe(spawnPos, Quaternion.identity);
        if (axe == null)
        {
            Debug.LogError("[AxeWeapon] Failed to spawn axe.");
            return;
        }

        axe.InitPool(_poolManager.AxePool);
        axe.SetAttacker(this);
        axe.Shoot(spawnPos, Vector2.right * direction, velocity.magnitude);
    }
}
