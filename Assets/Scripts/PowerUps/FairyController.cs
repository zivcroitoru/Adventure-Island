using UnityEngine;

public class FairyController : PickUp
{
    private static readonly Vector3 FairyOffset = new(0.5f, 1f, 0);

    public override void Collect(GameObject target)
    {
        OnPickUp(target);
        // Don't destroy here — FairyPowerUp handles cleanup
    }

    protected override void OnPickUp(GameObject player)
    {
        if (AlreadyAttachedTo(player))
        {
            Debug.Log("[FairyController] Player already has a fairy. Resetting timer.");
            ApplyPowerUp(player);
            Destroy(gameObject);
            return;
        }

        AttachToPlayer(player);
        ApplyPowerUp(player);
    }

    private bool AlreadyAttachedTo(GameObject player)
    {
        var existing = player.GetComponentInChildren<FairyController>();
        return existing != null && existing != this;
    }

    private void AttachToPlayer(GameObject player)
    {
        transform.SetParent(player.transform);
        transform.localPosition = FairyOffset;

        // Flip fairy to match player direction
        var scale = transform.localScale;
        scale.x = Mathf.Sign(player.transform.localScale.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        Debug.Log($"[FairyController] Attached to: {player.name} at {FairyOffset}");
    }

    private void ApplyPowerUp(GameObject player)
    {
        if (player.TryGetComponent<PlayerPowerUp>(out var powerUpHandler))
        {
            powerUpHandler.CollectPowerUp(new FairyPowerUp(gameObject));
            Debug.Log("[FairyController] Power-up collected.");
        }
        else
        {
            Debug.LogError("[FairyController] PlayerPowerUp missing.");
        }
    }
}
