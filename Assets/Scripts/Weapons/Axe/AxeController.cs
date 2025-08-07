using UnityEngine;

public class AxeController : PickUp
{
    protected override void OnPickUp(GameObject player)
    {
        if (player == null)
            return;

        var powerUp = new AxePowerUp();
        var powerUpHandler = player.GetComponent<PlayerPowerUp>();

        if (powerUpHandler != null)
            powerUpHandler.CollectPowerUp(powerUp);

        var axe = player.GetComponentInChildren<AxeWeapon>();
        var weaponsHandler = player.GetComponentInChildren<WeaponsHandler>();

        if (axe != null && weaponsHandler != null)
            weaponsHandler.Equip(axe);
    }
}
