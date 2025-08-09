using UnityEngine;

public class FairyPowerUp : IPowerUp
{
    private readonly GameObject _fairy;

    public FairyPowerUp(GameObject fairy)
    {
        _fairy = fairy;
    }

    public void ApplyPowerUp(GameObject player)
    {
        if (player.TryGetComponent<FairyInvinciblePowerUp>(out var invincible))
        {
    invincible.ActivateInvincibility(null, () =>
    {
        Object.Destroy(_fairy);
    });
            Debug.Log("[FairyPowerUp] Invincibility applied.");
        }
        else
        {
            Debug.LogError("[FairyPowerUp] Player missing FairyInvinciblePowerUp.");
        }
    }
}
