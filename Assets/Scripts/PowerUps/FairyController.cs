using UnityEngine;

public class FairyController : PickUp
{
    public override void Collect(GameObject target)
    {
        OnPickUp(target);
        // Do not destroy here — FairyPowerUp will destroy when done
    }

    protected override void OnPickUp(GameObject player)
    {
        Debug.Log($"[FairyController] Picked up by: {player.name}");

        // Check if a fairy is already attached to the player
        var existingFairy = player.GetComponentInChildren<FairyController>();
        if (existingFairy != null && existingFairy != this)
        {
            Debug.Log("[FairyController] Player already has a fairy. Resetting timer.");
            // Still apply power-up to reset timer, then destroy this one
            ApplyPowerUp(player);
            Destroy(gameObject);
            return;
        }

        // Parent the fairy to the player
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0.5f, 1f, 0);
        Debug.Log($"[FairyController] Parent set to: {transform.parent.name}, localPosition: {transform.localPosition}");

        ApplyPowerUp(player);
    }

    private void ApplyPowerUp(GameObject player)
    {
        IPowerUp powerUp = new FairyPowerUp(gameObject);
        var powerUpHandler = player.GetComponent<PlayerPowerUp>();

        if (powerUpHandler != null)
        {
            powerUpHandler.CollectPowerUp(powerUp);
            Debug.Log("[FairyController] Power-up collected.");
        }
        else
        {
            Debug.LogError("[FairyController] PlayerPowerUp component missing.");
        }
    }
}