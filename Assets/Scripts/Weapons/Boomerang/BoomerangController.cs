using UnityEngine;

public class BoomerangController : PickUp
{
    protected override void OnPickUp(GameObject player)
    {
        Debug.Log("[BoomerangController] OnPickUp triggered.");

        if (player == null)
        {
            Debug.LogWarning("[BoomerangController] Player reference is null!");
            return;
        }

        // Apply boomerang power-up
        var powerUp = new BoomerangPowerUp();
        var powerUpHandler = player.GetComponent<PlayerPowerUp>();

        if (powerUpHandler != null)
        {
            Debug.Log("[BoomerangController] Boomerang power-up collected!");
            powerUpHandler.CollectPowerUp(powerUp);
        }
        else
        {
            Debug.LogWarning("[BoomerangController] Player does not have a PlayerPowerUp component!");
        }

        // Equip the boomerang weapon via RideController
        if (player.TryGetComponent(out RideController ride))
        {
            ride.EquipWeapon(GetComponent<IWeapon>());
            Debug.Log("[BoomerangController] Weapon equipped via RideController.");
        }
        else
        {
            Debug.LogWarning("[BoomerangController] RideController not found on player.");
        }
    }
}
