using UnityEngine;

public abstract class AnimalBase : AnimatorAttackerBase
{
    // === FIELDS ===
    [Header("Animal Config")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private AnimatorOverrideController overrideController;

    // === STATE ===
    protected GameObject rider;

    // === PROPERTIES ===
    public GameObject Rider => rider;
    public virtual AnimatorOverrideController GetOverrideController() => overrideController;
    public virtual Collider2D GetGroundCheckCollider() => GetComponent<Collider2D>();

    // === LIFECYCLE ===
    public virtual void OnCollect(GameObject player)
    {
        if (player.TryGetComponent(out RideController rideController))
        {
            rideController.SwitchAnimal(this);
        }
    }

    public virtual void Mount(GameObject player)
    {
        rider = player;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        OnMounted();
    }

    public virtual void Dismount()
    {
        transform.SetParent(null);
        rider = null;
        OnDismounted();
    }

    // === ATTACK ===
    public void TriggerAttackHitbox()
    {
        if (!attackPrefab)
        {
            Debug.LogWarning("[AnimalBase] No attackPrefab assigned!");
            return;
        }

        GameObject hitbox = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        if (hitbox.TryGetComponent(out AnimalAttack attack))
        {
            attack.Owner = this;
        }

        Destroy(hitbox, 0.3f); // basic lifetime control
    }

    // === HOOKS ===
    protected virtual void OnMounted() { }
    protected virtual void OnDismounted() { }

    // === EXTENSION ===
    public abstract bool CanDestroy(ObstacleType type);
}
