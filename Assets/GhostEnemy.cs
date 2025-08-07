using UnityEngine;

[DisallowMultipleComponent]
public sealed class GhostEnemy : EnemyController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
        {
            // Block ghost kill if it's only temporary invincibility
            if (inv is FairyInvinciblePowerUp fairy && fairy.IsTemporaryOnly)
                return;

            Die(); // 👻 Dead
        }
    }

    public override void TakeDamage(int amount)
    {
        Debug.Log("[GhostEnemy] 👻 Ignored non-contact damage.");
    }
}
