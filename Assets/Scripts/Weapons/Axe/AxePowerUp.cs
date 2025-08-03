using UnityEngine;

public class AxePowerUp : IPowerUp
{
public void ApplyPowerUp(GameObject player)
{
    var handler = player.GetComponentInChildren<WeaponsHandler>();
    var axe = player.GetComponentInChildren<AxeWeapon>();
    handler?.Equip(axe);
}

}
