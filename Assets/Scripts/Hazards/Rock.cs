using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public sealed class Rock : MonoBehaviour, IObstacle, IDamageable
{
    [Header("Obstacle Damage")]
    [SerializeField] private int contactDamage = 3;
    [SerializeField] private int ridingDamage = 1;

    [Header("Type")]
    [SerializeField] private ObstacleType type = ObstacleType.Rock;

    public int ContactDamage => contactDamage;
    public int RidingDamage => ridingDamage;
    public ObstacleType Type => type;

    /* ---------- IObstacle ---------- */
    public void DestroyObstacle()
    {
        Debug.Log($"[Rock] DestroyObstacle called on {name}");
        Destroy(gameObject);
    }

    /* ---------- IDamageable ---------- */
   public void TakeDamage(int amount, GameObject dealer)
{
    if (!dealer) return;

    // --- Boomerang detection ---
    bool fromBoomerang =
        dealer.GetComponent<ProjectileBoomerang>() ||
        dealer.GetComponentInParent<ProjectileBoomerang>() ||
        dealer.GetComponentInChildren<ProjectileBoomerang>();

    // --- Other breakable projectile types ---
    bool fromSpark =
        dealer.GetComponent<ProjectileSpark>() ||
        dealer.GetComponentInParent<ProjectileSpark>() ||
        dealer.GetComponentInChildren<ProjectileSpark>();

    bool fromFire =
        dealer.GetComponent<ProjectileFire>() ||
        dealer.GetComponentInParent<ProjectileFire>() ||
        dealer.GetComponentInChildren<ProjectileFire>();

    if (fromBoomerang || fromSpark || fromFire)
    {
        Debug.Log($"[Rock] Destroyed by {dealer.name} ({dealer.tag})");
        DestroyObstacle();
    }
}

    /* ---------- Deal damage to player ---------- */
    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        Debug.Log($"[Rock] OnTriggerEnter2D with {target.name} (layer {target.layer})");
        TryDealContactDamage(target);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var target = col.rigidbody ? col.rigidbody.gameObject : col.collider.gameObject;
        Debug.Log($"[Rock] OnCollisionEnter2D with {target.name} (layer {target.layer})");
        TryDealContactDamage(target);
    }

    private void TryDealContactDamage(GameObject hit)
    {
        if (!hit) return;

        // Check if player is riding an animal
        var rider = ResolveRider(hit);
        var realTarget = rider ?? hit;

        if (realTarget.transform.root == transform.root)
        {
            Debug.Log("[Rock] Hit self/same root — ignoring.");
            return;
        }

        int amount = rider ? ridingDamage : contactDamage;
        Debug.Log($"[Rock] Attempting to deal {amount} damage to {realTarget.name}");

        Damage.Deal(amount, gameObject, realTarget);
    }

    private GameObject ResolveRider(GameObject hit)
    {
        var animal = hit.GetComponent<AnimalBase>() ?? hit.GetComponentInParent<AnimalBase>();
        if (animal != null && animal.Rider != null)
        {
            Debug.Log($"[Rock] Target is riding {animal.name}, rider is {animal.Rider.name}");
            return animal.Rider;
        }
        return null;
    }
}
