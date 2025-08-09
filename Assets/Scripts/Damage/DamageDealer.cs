using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int  fallbackDamage       = 20;   // weapon/projectile damage
    [SerializeField] bool destroyAfterHit      = false;

    [Header("Modes")]
    [SerializeField] bool isWeaponOrProjectile = false; // else: body/hurtbox
    [SerializeField] bool onlyWhenInvincible   = false; // ram hitbox

    [Header("Debug")]
    [SerializeField] bool debug = true;

    void Log(string msg) { if (debug) Debug.Log($"[DamageDealer:{name}|root={transform.root.name}] {msg}", this); }
    void Warn(string msg){ if (debug) Debug.LogWarning($"[DamageDealer:{name}] {msg}", this); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTarget(other);
        if (!target || target == gameObject) { Log("Skip: null or self"); return; }

        // If target is an obstacle, log its type (helps catch FIRE cases)
        if (target.TryGetComponent<IObstacle>(out var targetObs))
            Log($"Hit obstacle target={target.name} type={targetObs.Type}");

        // Must have something damageable
        var damageable = target.GetComponentInParent<IDamageable>();
        if (damageable == null) { Log($"Skip: no IDamageable on {target.name}"); return; }

        // Gate: only when invincible (for ram/hurtboxes)
        if (onlyWhenInvincible)
        {
            bool inv = IsAnyInvincibleActiveOnSelfHierarchy();
            Log($"onlyWhenInvincible={onlyWhenInvincible} → invActive={inv}");
            if (!inv) return;
        }

        // Compute damage
        int amount = CalculateDamageAgainst(target);
        Log($"Calculated damage={amount} (isWeaponOrProjectile={isWeaponOrProjectile})");
        if (amount <= 0) { Log("Skip: damage <= 0"); return; }

        // Safety: NEVER damage Fire (even if it ever becomes IDamageable)
        if (target.TryGetComponent<IObstacle>(out var obs) && obs.Type == ObstacleType.Fire)
        {
            Warn("Blocked: target is FIRE. No damage dealt.");
            return;
        }

        // Deal damage
        Log($"Deal → amount={amount} to {target.name}");
        Damage.Deal(amount, gameObject, target);

        // Optional destroy self (projectiles), but not when THIS is an obstacle (e.g., Fire)
        if (destroyAfterHit && !TryGetComponent<IObstacle>(out _))
        {
            Log("destroyAfterHit → Destroy(self)");
            Destroy(gameObject);
        }
    }

    private GameObject ResolveTarget(Collider2D hit)
    {
        // Rider shield: hitting an animal should target its rider
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
        {
            Log($"ResolveTarget: hit animal {hit.name} → rider {animal.Rider.name}");
            return animal.Rider;
        }

        var root = hit.attachedRigidbody ? hit.attachedRigidbody.gameObject : hit.gameObject;

        // Prefer the object that actually holds IDamageable
        var dmg = root.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            var go = ((Component)dmg).gameObject;
            Log($"ResolveTarget: chose IDamageable parent {go.name}");
            return go;
        }

        Log($"ResolveTarget: chose root {root.name}");
        return root;
    }

    private int CalculateDamageAgainst(GameObject target)
    {
        // If THIS object is an obstacle → use contact/riding numbers
        if (TryGetComponent<IObstacle>(out var selfObs))
        {
            bool isRiding = target.TryGetComponent<RideController>(out var rc) && rc.IsRiding;
            int dmg = isRiding ? selfObs.RidingDamage : selfObs.ContactDamage;
            Log($"CalculateDamage: self is Obstacle({selfObs.Type}) isRiding={isRiding} → {dmg}");
            return dmg;
        }

        // Only weapons/projectiles deal fallbackDamage
        if (isWeaponOrProjectile)
        {
            Log($"CalculateDamage: weapon/projectile → {fallbackDamage}");
            return fallbackDamage;
        }

        Log("CalculateDamage: non-weapon body → 0");
        return 0;
    }

    private bool IsAnyInvincibleActiveOnSelfHierarchy()
    {
        // self
        if (TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible) return true;

        // children
        var kids = GetComponentsInChildren<IInvincible>(true);
        for (int i = 0; i < kids.Length; i++) if (kids[i].IsInvincible) return true;

        // parents
        var pars = GetComponentsInParent<IInvincible>(true);
        for (int i = 0; i < pars.Length; i++) if (pars[i].IsInvincible) return true;

        return false;
    }
}
