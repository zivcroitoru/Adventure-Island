using UnityEngine;
using VContainer;

public class AxeWeapon : BaseWeapon
{
    [Inject] private AxePoolManager _axePoolManager;

    protected override void Fire()
    {
        if (_axePoolManager == null)
        {
            Debug.LogError("[AxeWeapon] AxePoolManager is null!");
            return;
        }

        GameObject axeGO = _axePoolManager.GetPooledAxe();
        if (axeGO == null)
        {
            Debug.LogError("[AxeWeapon] No available axe from pool!");
            return;
        }

        Debug.Log("[AxeWeapon] Axe obtained from pool.");

        // 🧭 Get player's position (parent), fallback to self
        Vector3 spawnPos = transform.parent ? transform.parent.position : transform.position;
        spawnPos.z = 0f;
        axeGO.transform.position = spawnPos;

        // ↔ Set direction based on player's scale
        float direction = transform.parent != null && transform.parent.localScale.x < 0 ? -1f : 1f;
        axeGO.transform.localScale = new Vector3(direction, 1f, 1f);

        axeGO.SetActive(true);

        if (axeGO.TryGetComponent(out ProjectileAxe projectile))
        {
            Rigidbody2D playerRb = transform.parent?.GetComponent<Rigidbody2D>();
            projectile.playerVelocity = playerRb ? playerRb.velocity : Vector2.zero;

            Debug.Log("[AxeWeapon] Calling projectile.Shoot()");
            projectile.Shoot(direction);
        }
        else
        {
            Debug.LogError("[AxeWeapon] Axe GameObject missing ProjectileAxe component!");
        }
    }
}
