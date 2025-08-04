using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    [Header("Animal Config")]
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
        AdjustRiderCollider(-0.5f);
        OnMounted();
    }

    public virtual void Dismount()
    {
        OnDismounted();

        if (rider != null && rider.TryGetComponent(out RideController rideController))
        {
            rideController.ResetAnimatorToBase();
        }

        AdjustRiderCollider(0f);
        DetachFromPlayer();
        rider = null;

        Destroy(gameObject);
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

    private void AdjustRiderCollider(float offsetY)
    {
        if (rider != null && rider.TryGetComponent(out CircleCollider2D collider))
        {
            collider.offset = new Vector2(collider.offset.x, offsetY);
        }
    }

    // === Obstacle & Enemy Logic ===
    public virtual bool CanDestroy(ObstacleType type)
    {
        return type != ObstacleType.Fire;
    }

    // public virtual bool CanHurtEnemy(EnemyType type)
    // {
    //     return type != EnemyType.Ghost;
    // }

    // === Lifecycle Hooks ===
    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }
}
