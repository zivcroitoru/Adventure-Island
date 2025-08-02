using UnityEngine;

public class AxePowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        AxeWeapon axeWeapon = player.GetComponentInChildren<AxeWeapon>();
        if (axeWeapon != null)
            axeWeapon.Equip();
    }
}
