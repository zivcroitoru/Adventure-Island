using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int  fallbackDamage   = 20;   // for real weapons/projectiles
    [SerializeField] bool destroyAfterHit  = false;
    [SerializeField] bool onlyWhenInvincible = false; // set true on “ram” hitboxes

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTarget(other);
        if (target == null || target == gameObject) return;

        // Must have something to damage
        var damageable = target.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        // Optional gate: only allow this hitbox when invincible/ramming
        if (onlyWhenInvincible && !(TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible))
            return;

        int amount = CalculateDamageAgainst(target);
        if (amount <= 0) return;

        Damage.Deal(amount, gameObject, target);

        if (destroyAfterHit && !TryGetComponent<IObstacle>(out _))
            Destroy(gameObject);
    }

    private GameObject ResolveTarget(Collider2D hit)
    {
        // If we hit a mount with a rider, damage the rider (the player)
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
            return animal.Rider;

        // Prefer the rigidbody root, then the parent that actually has IDamageable
        var root = hit.attachedRigidbody ? hit.attachedRigidbody.gameObject : hit.gameObject;
        var dmg  = root.GetComponentInParent<IDamageable>();
        return dmg != null ? ((Component)dmg).gameObject : root;
    }

    private int CalculateDamageAgainst(GameObject target)
    {
        // If THIS is an obstacle, use contact/riding logic
        if (TryGetComponent<IObstacle>(out var obs))
        {
            bool isRiding = target.TryGetComponent<RideController>(out var rc) && rc.IsRiding;
            return isRiding ? obs.RidingDamage : obs.ContactDamage;
        }

        // If THIS is a projectile/weapon → fallbackDamage
        if (GetComponent<BaseProjectile>() != null || GetComponent<IDamageDealer>() != null)
            return fallbackDamage;

        // Otherwise (e.g., player/mount body) → no damage
        return 0;
    }
}
