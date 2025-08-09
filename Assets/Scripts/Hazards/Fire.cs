using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageDealer))] // lets Damage.Deal run on contact
public sealed class Fire : MonoBehaviour, IObstacle, IResettable
{
    [Header("Damage")]
    [SerializeField] private int contactDamage = 999;
    [SerializeField] private int ridingDamage  = 0;

    [Header("Setup")]
    [Tooltip("Ensure this stays a trigger so InvincibleObstacleBreaker can catch it cleanly.")]
    [SerializeField] private bool forceTrigger = true;

    // IObstacle
    public ObstacleType Type        => ObstacleType.Fire;
    public int          ContactDamage => contactDamage;
    public int          RidingDamage  => ridingDamage;

    void Awake()
    {
        // Make sure we’re a trigger (player usually has the Rigidbody2D).
        var col = GetComponent<Collider2D>();
        if (col && forceTrigger) col.isTrigger = true;
    }

    // IObstacle: external destruction (breaker / level logic calls this)
    public void DestroyObstacle()
    {
        // No invincibility checks here—breaker handles that.
        Destroy(gameObject);
    }

    // IResettable: revive this hazard on level reset
    public void ResetState()
    {
        // Reactivate + re-enable visuals/colliders in case something turned them off
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = true;

        var sr = GetComponentInChildren<SpriteRenderer>(true);
        if (sr) sr.enabled = true;
    }
}
