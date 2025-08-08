using UnityEngine;

public interface IDamageable
{
    // Dealer is passed so the target can decide (e.g., which projectile type hit it).
    void TakeDamage(int amount, GameObject dealer);
}
