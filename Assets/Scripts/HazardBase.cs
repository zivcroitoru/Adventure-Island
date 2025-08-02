using UnityEngine;

/// <summary>
/// Base class for environmental hazards that deal damage or destroy entities on contact.
/// </summary>
public abstract class HazardBase : MonoBehaviour
{
    [SerializeField] protected int damage = 20; // Damage to apply to IDamageable targets

    /// <summary>
    /// Triggered when another collider enters this hazard's trigger area.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[HazardBase] Trigger entered by: {other.gameObject.name}");

        // If collision is blocked by override logic, skip
        if (IsBlocked(other))
        {
            Debug.Log("[HazardBase] Collision blocked.");
            return;
        }

        // If this collision destroys a mount (e.g. dino, vehicle), handle and stop
        if (TryHandleMountDestruction(other))
        {
            Debug.Log("[HazardBase] Mount destruction handled.");
            return;
        }

        // If the collider is a projectile that should be destroyed, handle and stop
        if (TryHandleProjectileDestruction(other))
        {
            Debug.Log("[HazardBase] Projectile destruction handled.");
            return;
        }

        // Otherwise, try applying damage
        TryApplyDamage(other);
    }

    /// <summary>
    /// Optional override to prevent handling this collider.
    /// </summary>
    protected virtual bool IsBlocked(Collider2D other)
    {
        // Example: could check tag or layer to ignore certain objects
        return false;
    }

    /// <summary>
    /// Optional override for destroying mounts (e.g. if player is riding).
    /// </summary>
    protected virtual bool TryHandleMountDestruction(Collider2D other)
    {
        return false;
    }

    /// <summary>
    /// Optional override for destroying projectiles like axes or fireballs.
    /// </summary>
    protected virtual bool TryHandleProjectileDestruction(Collider2D other)
    {
        return false;
    }

    /// <summary>
    /// Attempts to apply damage to a damageable target.
    /// </summary>
    protected virtual void TryApplyDamage(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log($"[HazardBase] Applying {damage} damage to: {other.name}");
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"[HazardBase] No IDamageable found on: {other.name}");
        }
    }
}
