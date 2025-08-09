using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central damage router. Minimal GC, clear gates, loud (optional) debug.
/// </summary>
public static class Damage
{
    // Toggle logs without touching call sites.
    const bool DEBUG_LOGS = false;

    // Reused buffers to avoid per-call allocations.
    static readonly List<IInvincible> _invBuf = new List<IInvincible>(4);

    /// <summary>
    /// Deal damage from dealer → rawTarget. Returns true if damage applied.
    /// Allowed dealers: IDamageDealer, IObstacle, BaseProjectile, IRamDamageSource.
    /// </summary>
    public static bool Deal(int amount, GameObject dealer, GameObject rawTarget)
    {
        // --- Fast rejects ---
        if (amount <= 0 || dealer == null || rawTarget == null) return false;
        if (!IsLegitDealer(dealer))
        {
            Log($"[Damage] Blocked: non-legit dealer {dealer.name}");
            return false;
        }

        // Pick a stable object to evaluate (usually the rigidbody root).
        var target = ResolveTargetRoot(rawTarget);
        if (target == null) return false;

        // No self hits / same character root.
        if (ReferenceEquals(target, dealer) || target.transform.root == dealer.transform.root)
        {
            Log($"[Damage] Blocked: self/root {dealer.name} → {target.name}");
            return false;
        }

        // Invincibility gates (on target or its hierarchy).
        if (IsHierarchyInvincible(target))
        {
            Log($"[Damage] Blocked: {target.name} is invincible");
            return false;
        }

        // Find something to damage.
        if (!target.TryGetComponent<IDamageable>(out var dmg))
            dmg = target.GetComponentInParent<IDamageable>();

        if (dmg == null)
        {
            Log($"[Damage] No IDamageable found on {target.name}");
            return false;
        }

        // Apply.
        Log($"[Damage] {dealer.name} → {target.name} : {amount}");
        dmg.TakeDamage(amount, dealer);
        return true;
    }

    // -------- Helpers --------

    /// <summary>Dealer whitelist.</summary>
    static bool IsLegitDealer(GameObject dealer)
    {
        // IDamageDealer: melee/hitboxes; IObstacle: hazards; BaseProjectile: bullets;
        // IRamDamageSource: your invincible ram breaker.
        return dealer.TryGetComponent<IDamageDealer>(out _) ||
               dealer.TryGetComponent<IObstacle>(out _) ||
               dealer.GetComponent<BaseProjectile>() != null;
    }

    /// <summary>
    /// Prefer rigidbody root to avoid child-collider weirdness.
    /// Falls back to transform.root.
    /// </summary>
    static GameObject ResolveTargetRoot(GameObject go)
    {
        if (go == null) return null;

        // If the hit had a Rigidbody2D anywhere below, use that root.
        if (go.TryGetComponent<Rigidbody2D>(out var rb) && rb != null)
            return rb.transform.root.gameObject;

        // Else, walk up to the topmost transform.
        var t = go.transform;
        while (t.parent != null) t = t.parent;
        return t.gameObject;
    }

    /// <summary>
    /// True if any IInvincible in target’s hierarchy is active.
    /// Uses non-alloc pattern with a shared buffer.
    /// </summary>
    static bool IsHierarchyInvincible(GameObject root)
    {
        // 1) Quick checks on the root itself.
        if (root.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible) return true;

        // 2) Scan parents (cheap: usually shallow).
        var t = root.transform.parent;
        while (t != null)
        {
            if (t.TryGetComponent<IInvincible>(out var pInv) && pInv.IsInvincible) return true;
            t = t.parent;
        }

        // 3) Children (use non-alloc GetComponentsInChildren into a reused list).
        _invBuf.Clear();
        root.GetComponentsInChildren(true, _invBuf);
        for (int i = 0; i < _invBuf.Count; i++)
            if (_invBuf[i] != null && _invBuf[i].IsInvincible) return true;

        return false;
    }

    static void Log(string msg)
    {
        if (DEBUG_LOGS) Debug.Log(msg);
    }
}
