using UnityEngine;


[DisallowMultipleComponent]
public sealed class FairyController : PickUp
{
    static readonly Vector3 LocalOffset = new(0.5f, 1f, 0f);

    FairyInvinciblePowerUp _inv;  

    public override void Collect(GameObject target) => OnPickUp(target);

    protected override void OnPickUp(GameObject player)
    {
        if (AlreadyAttachedTo(player))
        {
            ApplyPowerUp(player); 
            Destroy(gameObject);  
            return;
        }

        AttachToPlayer(player);
        ApplyPowerUp(player);     
        SubscribeToProvider(player);
    }

    void OnDestroy()
    {
        if (_inv != null)
        {
            _inv.PowerUpEnded -= HandlePowerUpEnded;
            _inv = null;
        }
    }

    // --- Helpers ---

    bool AlreadyAttachedTo(GameObject player)
    {
        var existing = player.GetComponentInChildren<FairyController>();
        return existing != null && existing != this;
    }

    void AttachToPlayer(GameObject player)
    {
        var pivot = player.transform.Find("Visual") ?? player.transform;
        transform.SetParent(pivot, false);
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x);
        transform.localScale = s;
        float dir = Mathf.Sign(pivot.lossyScale.x == 0f ? 1f : pivot.lossyScale.x);
        transform.localPosition = new Vector3(LocalOffset.x * dir, LocalOffset.y, LocalOffset.z);
    }

    void ApplyPowerUp(GameObject player)
    {
        if (player.TryGetComponent<PlayerPowerUp>(out var powerUpHandler))
        {
            powerUpHandler.CollectPowerUp(new FairyPowerUp(gameObject));
        }
    }

    void SubscribeToProvider(GameObject player)
    {
        if (!player.TryGetComponent<FairyInvinciblePowerUp>(out var inv))
        {
            inv = player.GetComponentInChildren<FairyInvinciblePowerUp>();
        }
        if (_inv != null) _inv.PowerUpEnded -= HandlePowerUpEnded;
        _inv = inv;
        _inv.PowerUpEnded += HandlePowerUpEnded;
    }

    void HandlePowerUpEnded()
    {
        if (_inv != null) _inv.PowerUpEnded -= HandlePowerUpEnded;
        _inv = null;
        Destroy(gameObject);
    }
}
