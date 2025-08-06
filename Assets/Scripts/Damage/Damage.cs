using UnityEngine;
using System;

public static class Damage
{
    // ----- rule chain -----
    private static readonly Func<DamageContext, bool>[] Chain =
    {
        // Invincible player destroys obstacle
        ctx =>
        {
            if (ctx.Flags.HasFlag(DamageFlags.Invincible) &&
                ctx.Dealer.TryGetComponent<IObstacle>(out var obs) &&
                ctx.Target.CompareTag("Player"))
            {
                obs.DestroyObstacle();
                return true;
            }
            return false;
        },

        // Riding (not invincible) → dismount
        ctx =>
        {
            if (ctx.Flags.HasFlag(DamageFlags.Riding) &&
                !ctx.Flags.HasFlag(DamageFlags.Invincible) &&
                ctx.Target.TryGetComponent<RideController>(out var rc))
            {
                rc.DismountCurrentAnimal();
                return true;
            }
            return false;
        },

        // Default → apply damage if possible
        ctx =>
        {
            if (ctx.Target.TryGetComponent<IDamageable>(out var dmg))
                dmg.TakeDamage(ctx.Amount);
            return true;
        }
    };

    // ----- API -----
    public static void Deal(int amount, GameObject dealerGO, GameObject targetGO)
    {
        var flags = DamageFlags.None;
        if (targetGO.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
            flags |= DamageFlags.Invincible;
        if (targetGO.TryGetComponent<RideController>(out var rc) && rc.IsRiding)
            flags |= DamageFlags.Riding;

        var ctx = new DamageContext
        {
            Dealer  = dealerGO,
            Target  = targetGO,
            Amount  = amount,
            Flags   = flags
        };

        foreach (var rule in Chain)
            if (rule(ctx)) break;
    }
}
