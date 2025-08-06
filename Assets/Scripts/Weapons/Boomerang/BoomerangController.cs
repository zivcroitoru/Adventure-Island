using UnityEngine;

public class BoomerangController : PickUp
{
    protected override void OnPickUp(GameObject player)
    {
        if (player == null) return;

        var powerUp = new BoomerangPowerUp();

        if (player.TryGetComponent(out PlayerPowerUp powerUpHandler))
            powerUpHandler.CollectPowerUp(powerUp);

        if (player.TryGetComponent(out RideController ride))
        {
            var weapon = player.GetComponentInChildren<BoomerangWeapon>();
            ride.EquipWeapon(weapon);
        }
    }
}
