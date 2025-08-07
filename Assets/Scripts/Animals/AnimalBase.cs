using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    [Header("Animal Config")]
    [SerializeField] private AnimatorOverrideController overrideController;

    protected GameObject rider;

    public GameObject Rider => rider;

    // === Animator ===
    public virtual AnimatorOverrideController GetOverrideController() => overrideController;

    // === Collection & Mounting ===
    public virtual void OnCollect(GameObject player)
    {
        if (player.TryGetComponent<RideController>(out var rideController))
            rideController.SwitchAnimal(this);
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
    // Skip dismount if player is invincible
    if (rider != null &&
        rider.TryGetComponent<IInvincible>(out var invincible) &&
        invincible.IsInvincible)
    {
        Debug.Log("[AnimalBase] Cannot dismount, rider is invincible.");
        return;  // Do not allow dismount if the player is invincible
    }

    // Continue with the normal dismount process if player is not invincible
    OnDismounted();

    if (rider == null) return;

    if (rider.TryGetComponent<RideController>(out var rideController))
        rideController.ResetAnimatorToBase();

    AdjustRiderCollider(0f);
    DetachFromPlayer();

    rider = null;
    Destroy(gameObject);
}


    // === Attach & Detach ===
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
        protected Vector2 GetFacingDirection()
    {
        return rider?.transform.localScale.x >= 0f ? Vector2.right : Vector2.left;
    }

    protected float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;
        return 0f;
    }


    // === Capabilities ===
    public virtual bool CanDestroy(ObstacleType type) => type != ObstacleType.Fire;
    public virtual bool CanHurtEnemy(EnemyType type) => type != EnemyType.Ghost;

    // === Hooks ===
    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }
}
