using UnityEngine;

public class AxeController : PickUp
{
    protected override void OnPickUp(GameObject player)
    {
        Debug.Log("[AxeController] OnPickUp triggered.");

        if (player == null)
        {
            Debug.LogWarning("[AxeController] Player reference is null!");
            return;
        }

        var powerUp = new AxePowerUp();
        var powerUpHandler = player.GetComponent<PlayerPowerUp>();

        if (powerUpHandler != null)
        {
            Debug.Log("[AxeController] Axe power-up collected!");
            powerUpHandler.CollectPowerUp(powerUp);
        }
        else
        {
            Debug.LogWarning("[AxeController] Player does not have a PlayerPowerUp component!");
        }
    }
}
