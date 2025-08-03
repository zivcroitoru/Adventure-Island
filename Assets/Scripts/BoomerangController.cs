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
    }
}
