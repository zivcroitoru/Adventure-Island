using UnityEngine;
using VContainer;

public class AxePowerUp : IPowerUp
{
    [Inject] private ProjectileAxePool _pool;

    public void ApplyPowerUp(GameObject player)
    {
        var handler = player.GetComponentInChildren<WeaponsHandler>();
        var axe = player.GetComponentInChildren<AxeWeapon>();

        if (axe != null)
        {
            Debug.Log($"[AxePowerUp] Found AxeWeapon instance {axe.GetInstanceID()}");
            if (axe.PoolIsSet()) // <-- implement this method in AxeWeapon (see below)
            {
                Debug.Log($"[AxePowerUp] Pool already injected in {axe.GetInstanceID()}");
            }
            else
            {
                axe.Construct(_pool);
                Debug.Log($"[AxePowerUp] Pool injected into AxeWeapon {axe.GetInstanceID()}");
            }
        }
        else
        {
            Debug.LogWarning("[AxePowerUp] No AxeWeapon found!");
        }

        handler?.Equip(axe);
    }
}
