using UnityEngine;

public class FairyController : PickUp
{
    private static readonly Vector3 FairyOffset = new(0.5f, 1f, 0);

    public override void Collect(GameObject target)
    {
        OnPickUp(target);
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
    var pivot = player.transform.Find("Visual") ?? player.transform;
    transform.SetParent(pivot, false);
    var s = transform.localScale; 
    s.x = Mathf.Abs(s.x);
    transform.localScale = s;
    float dir = Mathf.Sign(pivot.lossyScale.x);
    transform.localPosition = new Vector3(0.5f * dir, 1f, 0f);
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
