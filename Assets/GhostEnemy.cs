using UnityEngine;

[DisallowMultipleComponent]
public sealed class GhostEnemy : EnemyController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Check if the player is invincible
        if (other.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
        {
            // Block ghost kill if it's only temporary invincibility
            if (inv is FairyInvinciblePowerUp fairy && fairy.IsTemporaryOnly)
                return;

            // If the player is invincible (permanently), the ghost dies
            Die(); // 👻 Dead
        }
    }

    public override void TakeDamage(int amount)
    {
        // Only take damage if the player is invincible
        if (TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
        {
            // Apply damage logic here (if needed)
            Debug.Log("[GhostEnemy] 👻 Took damage from invincible player.");
        }
        else
        {
            Debug.Log("[GhostEnemy] 👻 Ignored damage from non-invincible player.");
        }
    }
}
