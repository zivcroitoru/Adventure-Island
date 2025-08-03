using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    [Header("Animal Config")]
    [SerializeField] protected GameObject attackPrefab;
    [SerializeField] private AnimatorOverrideController overrideController;

    protected GameObject rider;

    // === Properties ===
    public GameObject Rider => rider;
    public virtual AnimatorOverrideController GetOverrideController() => overrideController;

    // === Collection ===
    public virtual void OnCollect(GameObject player)
    {
        if (player.TryGetComponent(out RideController rideController))
        {
            rideController.SwitchAnimal(this);
        }
    }

    // === Mounting ===
    public virtual void Mount(GameObject player)
    {
        if (player == null) return;

        rider = player;
        AttachToPlayer();
        AdjustRiderCollider();
        OnMounted();
    }

    public virtual void Dismount()
    {
        DetachFromPlayer();
        rider = null;
        OnDismounted();
    }

    private void AttachToPlayer()
    {
        transform.SetParent(rider.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void DetachFromPlayer()
    {
        transform.SetParent(null);
    }

    private void AdjustRiderCollider()
    {
        if (rider.TryGetComponent(out CircleCollider2D collider))
        {
            collider.offset = new Vector2(collider.offset.x, -0.5f);
        }
        else
        {
            Debug.LogWarning("[AnimalBase] Rider has no CircleCollider2D.");
        }
    }

    // === Attack ===
    public void TriggerAttackHitbox()
    {
        if (attackPrefab == null)
        {
            Debug.LogWarning("[AnimalBase] No attackPrefab assigned.");
            return;
        }

        var hitbox = Instantiate(attackPrefab, transform.position, Quaternion.identity);

        if (hitbox.TryGetComponent(out AnimalAttack attack))
        {
            attack.Owner = this;
        }

        Destroy(hitbox, 0.3f);
    }

    // === Lifecycle Hooks ===
    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }

    // === Custom Logic ===
    public abstract bool CanDestroy(ObstacleType type);
}
