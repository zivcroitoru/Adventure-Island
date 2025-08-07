using UnityEngine;
using VContainer;

public class AxePowerUp : IPowerUp
{
    [Inject] private ProjectileAxePool _pool;

    public void ApplyPowerUp(GameObject player)
    {
        var handler = player.GetComponentInChildren<WeaponsHandler>();
        var axe = player.GetComponentInChildren<AxeWeapon>();

        if (axe != null && !axe.PoolIsSet())
            axe.Construct(_pool);

        handler?.Equip(axe);
    }
}
