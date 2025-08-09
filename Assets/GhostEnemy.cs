using UnityEngine;

[DisallowMultipleComponent]
public sealed class GhostEnemy : EnemyController
{
    // Kill instantly if the *dealer* (the player rammer) is invincible.
    public override void TakeDamage(int amount, GameObject dealer)
    {
        bool dealerInv =
            dealer && (dealer.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible
                    || dealer.GetComponentInParent<IInvincible>()?.IsInvincible == true);

        if (dealerInv)
        {
            Debug.Log("[GhostEnemy] Dealer is invincible → die now.");
            ApplyDamage(int.MaxValue);
            return;
        }

        base.TakeDamage(amount, dealer); // respects lethal amount when not invincible
    }

    // If anyone calls the overload without dealer, fall back to normal damage
    public override void TakeDamage(int amount) => base.TakeDamage(amount);
}
