using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    [Header("Animal Config")]
    [SerializeField] private AnimatorOverrideController overrideController;

    protected GameObject rider;

    public GameObject Rider => rider;
    public virtual AnimatorOverrideController GetOverrideController() => overrideController;

    public virtual void OnCollect(GameObject player)
    {
        if (player.TryGetComponent<RideController>(out var rideController))
        {
            rideController.SwitchAnimal(this);
        }
    }

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

        if (rider == null) return;

        if (rider.TryGetComponent<RideController>(out var rideController))
            rideController.ResetAnimatorToBase();

        AdjustRiderCollider(0f);
        DetachFromPlayer();

        if (rider.TryGetComponent<IInvincible>(out var invincible) && invincible.IsInvincible)
        {
            rider = null;
            return;
        }

        rider = null;
        Destroy(gameObject);
    }

    protected virtual void AttachToPlayer()
    {
        transform.SetParent(rider.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    protected virtual void DetachFromPlayer()
    {
        transform.SetParent(null);
    }

    protected virtual void AdjustRiderCollider(float offsetY)
    {
        if (rider != null && rider.TryGetComponent(out CircleCollider2D collider))
            collider.offset = new Vector2(collider.offset.x, offsetY);
    }

    public virtual bool CanDestroy(ObstacleType type) => true;
    public virtual bool CanHurtEnemy(EnemyType type) => type != EnemyType.Ghost;

    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }
}
