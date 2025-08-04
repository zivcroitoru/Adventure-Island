using UnityEngine;
using VContainer;

public class AxeWeapon : BaseWeapon, IAttacker
{
    [Inject] private AxePoolManager axePoolManager;
    [SerializeField] private float _projectileSpeed = 5f;

    protected override void Fire()
    {
        if (axePoolManager == null)
        {
            Debug.LogError("[AxeWeapon] AxePoolManager is null!");
            return;
        }

        GameObject axeGO = axePoolManager.GetPooledAxe();
        if (axeGO == null)
        {
            Debug.LogError("[AxeWeapon] No available axe from pool!");
            return;
        }

        Transform player = transform.parent;
        if (player == null)
        {
            Debug.LogWarning("[AxeWeapon] No parent found to determine direction/position.");
            return;
        }

        float direction = player.localScale.x < 0 ? -1f : 1f;
        Vector3 spawnPos = player.position;
        Vector2 velocity = player.TryGetComponent(out Rigidbody2D rb) ? rb.velocity : Vector2.zero;

        SetupProjectile(axeGO, spawnPos, direction);
        LaunchProjectile(axeGO, direction, velocity);

        Debug.Log("[AxeWeapon] Projectile launched.");
    }

    private void SetupProjectile(GameObject axeGO, Vector3 position, float direction)
    {
        position.z = 0f;
        axeGO.transform.position = position;
        axeGO.transform.localScale = new Vector3(direction, 1f, 1f);
        axeGO.SetActive(true);
    }

    private void LaunchProjectile(GameObject axeGO, float direction, Vector2 velocity)
    {
        if (!axeGO.TryGetComponent(out ProjectileAxe projectile))
        {
            Debug.LogError("[AxeWeapon] Axe GameObject missing ProjectileAxe component!");
            return;
        }

        projectile.Initialized(_projectileSpeed);
        projectile.SetDirection(direction);
        projectile.SetAttacker(this); // Animation handled by WeaponsHandler
        projectile.PlayerVelocity = velocity;
        projectile.Shoot();
    }
}
