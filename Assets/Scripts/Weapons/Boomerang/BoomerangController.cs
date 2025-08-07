using UnityEngine;

public class BoomerangController : PickUp
{
    protected override void OnPickUp(GameObject player)
    {
        if (player == null) return;

        var powerUp = new BoomerangPowerUp();

        if (player.TryGetComponent(out PlayerPowerUp powerUpHandler))
            powerUpHandler.CollectPowerUp(powerUp);

        var weapon = player.GetComponentInChildren<BoomerangWeapon>();
        var weaponsHandler = player.GetComponentInChildren<WeaponsHandler>();

        if (weapon != null && weaponsHandler != null)
            weaponsHandler.Equip(weapon);
    }
}
