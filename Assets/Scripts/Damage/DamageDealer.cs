using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int  fallbackDamage      = 20;   // for real weapons/projectiles
    [SerializeField] bool destroyAfterHit     = false;

    // ✅ Explicit: check this to treat this collider as a weapon/hitbox
    [SerializeField] bool isWeaponOrProjectile = false;

    // ✅ Ram/hurtbox should only work when rider is invincible (fairy etc.)
    [SerializeField] bool onlyWhenInvincible   = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTarget(other);
        if (!target || target == gameObject) return;

        // Must have something to damage
        var damageable = target.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        // Gate: ram/hurtbox only if ANY invincible on our root hierarchy is active
        if (onlyWhenInvincible && !IsAnyInvincibleActiveOnSelfHierarchy())
            return;

        int amount = CalculateDamageAgainst(target);
        if (amount <= 0) return;

        Damage.Deal(amount, gameObject, target);

        if (destroyAfterHit && !TryGetComponent<IObstacle>(out _))
            Destroy(gameObject);
    }

    private GameObject ResolveTarget(Collider2D hit)
    {
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
            return animal.Rider;

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

        // ✅ Only deal fallbackDamage if this collider is explicitly configured as a weapon
        if (isWeaponOrProjectile)
            return fallbackDamage;

        // Otherwise (player/mount body) → no damage
        return 0;
    }

    private bool IsAnyInvincibleActiveOnSelfHierarchy()
    {
        // check self + parents + children (covers Fairy on rider or mount)
        if (TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible) return true;
        foreach (var c in GetComponentsInChildren<IInvincible>(true)) if (c.IsInvincible) return true;
        foreach (var p in GetComponentsInParent<IInvincible>(true))  if (p.IsInvincible) return true;
        return false;
    }
}
