using UnityEngine;
using System;

public static class Damage
{
    // Chain of rules for resolving damage effects
    private static readonly Func<DamageContext, bool>[] Chain =
    {
        // Invincible player avoids damage (blocks damage completely)
        ctx =>
        {
            if (ctx.Flags.HasFlag(DamageFlags.Invincible) && ctx.Target.TryGetComponent<IDamageable>(out var dmg))
            {
                Debug.Log("[Damage] Player is invincible. Damage blocked.");
                return true;  // Block damage if the player is invincible
            }
            return false;
        },

        // Invincible player destroys obstacle
        ctx =>
        {
            if (ctx.Flags.HasFlag(DamageFlags.Invincible) &&
                ctx.Dealer.TryGetComponent<IObstacle>(out var obs))
            {
                obs.DestroyObstacle();
                return true;
            }
            return false;
        },

        // Riding player (not invincible) dismounts
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

        // Otherwise apply damage
        ctx =>
        {
            if (ctx.Target.TryGetComponent<IDamageable>(out var dmg))
                dmg.TakeDamage(ctx.Amount);
            return true;
        }
    };

    // Entry point
    public static void Deal(int amount, GameObject dealer, GameObject target)
    {
        var ctx = new DamageContext(dealer, target, amount);

        foreach (var rule in Chain)
        {
            if (rule(ctx))
                break;
        }
    }
}
