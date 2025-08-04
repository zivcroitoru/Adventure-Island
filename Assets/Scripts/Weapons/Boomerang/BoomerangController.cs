using UnityEngine;

public class BoomerangController : PickUp
{
    [SerializeField] private MonoBehaviour weaponComponent; // Must implement IWeapon

    protected override void OnPickUp(GameObject player)
    {
        Debug.Log("[BoomerangController] OnPickUp triggered.");

        if (player == null)
        {
            Debug.LogWarning("[BoomerangController] Player reference is null!");
            return;
        }

        // ✅ Apply power-up
        var powerUp = new BoomerangPowerUp();
        if (player.TryGetComponent(out PlayerPowerUp powerUpHandler))
        {
            powerUpHandler.CollectPowerUp(powerUp);
            Debug.Log("[BoomerangController] Boomerang power-up collected!");
        }
        else
        {
            Debug.LogWarning("[BoomerangController] Player missing PlayerPowerUp.");
        }

        // ✅ Equip weapon
        if (player.TryGetComponent(out RideController ride))
        {
            if (weaponComponent is IWeapon weapon)
            {
                ride.EquipWeapon(weapon);
                Debug.Log("[BoomerangController] Weapon equipped via RideController.");
            }
            else
            {
                Debug.LogWarning("[BoomerangController] Assigned component does not implement IWeapon.");
            }
        }
        else
        {
            Debug.LogWarning("[BoomerangController] RideController not found on player.");
        }
    }
}
