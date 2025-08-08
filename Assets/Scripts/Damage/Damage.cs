using UnityEngine;

public static class Damage
{
    public static void Deal(int amount, GameObject dealer, GameObject rawTarget)
    {
        if (!dealer || !rawTarget) return;

        var target = ResolveTarget(rawTarget);

        // Skip self / same root
        if (ReferenceEquals(target, dealer)) return;
        if (target.transform.root == dealer.transform.root) return;

        // Target invincible → block normal damage
        if (target.TryGetComponent<IInvincible>(out var targetInv) && targetInv.IsInvincible)
        {
            Debug.Log($"[Damage] Blocked: {target.name} is invincible. amount={amount}");
            return;
        }

        // === MAIN: find IDamageable on target OR ITS PARENTS ===
        if (target.TryGetComponent<IDamageable>(out var dmg) ||
            (dmg = target.GetComponentInParent<IDamageable>()) != null)
        {
            Debug.Log($"[Damage] Dealing {amount} from {dealer.name} -> {target.name}");
            dmg.TakeDamage(amount, dealer);
            return;
        }

        // Debug.Log($"[Damage] No IDamageable on {target.name} or its parents. amount={amount}, dealer={dealer.name}");
    }

    static GameObject ResolveTarget(GameObject hit)
    {
        // Send damage to rider if on an animal
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
            return animal.Rider;

        // Use RB root if present
        var rb = hit.GetComponent<Rigidbody2D>() ?? hit.GetComponentInParent<Rigidbody2D>();
        return rb ? rb.gameObject : hit;
    }
}
