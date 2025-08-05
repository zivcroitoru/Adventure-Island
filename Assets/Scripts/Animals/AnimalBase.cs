using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    [Header("Animal Config")]
    [SerializeField] private AnimatorOverrideController overrideController;

    protected GameObject rider;

    // === Public Properties ===
    public GameObject Rider => rider;
    public virtual AnimatorOverrideController GetOverrideController() => overrideController;

    // === Collection ===
    public virtual void OnCollect(GameObject player)
    {
        if (player.TryGetComponent<RideController>(out var rideController))
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
        AdjustRiderCollider(offsetY: -0.5f);
        OnMounted();
    }

    public virtual void Dismount()
    {


    Debug.Log($"[AnimalBase] Dismount called on {name}");
    Debug.Log("[AnimalBase] Call stack:\n" + System.Environment.StackTrace);



        OnDismounted();

        if (rider != null)
        {
            if (rider.TryGetComponent<RideController>(out var rideController))
            {
                rideController.ResetAnimatorToBase();
            }

            AdjustRiderCollider(offsetY: 0f);
            DetachFromPlayer();

            // ❗ Prevent destruction if player is invincible
            if (rider.TryGetComponent<IInvincible>(out var invincible) && invincible.IsInvincible)
            {
                rider = null;
                return;
            }

            rider = null;
            Destroy(gameObject);
        }
    }

    // === Attach/Detach ===
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
        if (rider != null && rider.TryGetComponent<CircleCollider2D>(out var collider))
        {
            collider.offset = new Vector2(collider.offset.x, offsetY);
        }
    }

    // === Obstacle & Enemy Logic ===
    public virtual bool CanDestroy(ObstacleType type) => type != ObstacleType.Fire;
    public virtual bool CanHurtEnemy(EnemyType type) => type != EnemyType.Ghost;

    // === Lifecycle Hooks ===
    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }
}
