using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class HazardBase : MonoBehaviour
{
    [SerializeField] protected int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldIgnore(other)) return;
        if (HandleAnimalMountDestruction(other)) return;
        if (HandleExternalDestruction(other)) return;

        ApplyDamagePipeline(other);
    }

    /// <summary>
    /// Override to skip specific interactions (e.g. other hazards, projectiles).
    /// </summary>
    protected virtual bool ShouldIgnore(Collider2D other) => false;

    /// <summary>
    /// Override if this hazard breaks the player's rideable animal.
    /// </summary>
    protected virtual bool HandleAnimalMountDestruction(Collider2D other) => false;

    /// <summary>
    /// Override if this hazard can be destroyed by projectiles or attacks.
    /// </summary>
    protected virtual bool HandleExternalDestruction(Collider2D other) => false;

    /// <summary>
    /// Main interaction logic: either destroy the hazard (if player is invincible) or damage the player.
    /// </summary>
    protected virtual void ApplyDamagePipeline(Collider2D other)
    {
        // Invincible players destroy the hazard
        if (other.TryGetComponent<IInvincible>(out var invincible) && invincible.IsInvincible)
        {
            if (TryGetComponent<IDamageable>(out var self))
            {
                self.TakeDamage(damage);
            }

            return;
        }

        // Otherwise, deal damage to the other object
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
        }
    }

}
