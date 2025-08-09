using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    static readonly List<IInvincible> _invBuf = new(4);

    public static bool Deal(int amount, GameObject dealer, GameObject rawTarget)
    {
        if (amount <= 0 || dealer == null || rawTarget == null) return false;
        if (!IsLegitDealer(dealer)) return false;

        var target = ResolveTargetRoot(rawTarget);
        if (target == null) return false;
        if (ReferenceEquals(target, dealer) || target.transform.root == dealer.transform.root) return false;
        if (IsHierarchyInvincible(target)) return false;

        if (!target.TryGetComponent<IDamageable>(out var dmg))
            dmg = target.GetComponentInParent<IDamageable>();
        if (dmg == null) return false;

        dmg.TakeDamage(amount, dealer);
        return true;
    }

    static bool IsLegitDealer(GameObject d) =>
        d.TryGetComponent<IDamageDealer>(out _) ||
        d.TryGetComponent<IObstacle>(out _) ||
        d.GetComponent<BaseProjectile>() != null;

    static GameObject ResolveTargetRoot(GameObject go)
    {
        var rb = go.GetComponentInParent<Rigidbody2D>();
        return rb ? rb.transform.root.gameObject : go.transform.root.gameObject;
    }

    static bool IsHierarchyInvincible(GameObject root)
    {
        if (root.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible) return true;
        for (var t = root.transform.parent; t != null; t = t.parent)
            if (t.TryGetComponent<IInvincible>(out var pInv) && pInv.IsInvincible) return true;

        _invBuf.Clear();
        root.GetComponentsInChildren(true, _invBuf);
        for (int i = 0; i < _invBuf.Count; i++)
            if (_invBuf[i] != null && _invBuf[i].IsInvincible) return true;

        return false;
    }
}
