using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class HazardBase : MonoBehaviour
{
    [SerializeField] protected int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldIgnore(other)) return;
        if (HandleMountDestruction(other)) return;
        if (HandleDestructionByExternalHit(other)) return;
        TryApplyDamage(other);
    }

    protected virtual bool ShouldIgnore(Collider2D other) => false;

    protected virtual bool HandleMountDestruction(Collider2D other) => false;

    protected virtual bool HandleDestructionByExternalHit(Collider2D other) => false;

protected virtual void TryApplyDamage(Collider2D other)
{
    // 1. If invincible — destroy the hazard
    if (other.TryGetComponent<IInvincible>(out var invincible) && invincible.IsInvincible)
    {
        if (TryGetComponent<IDamageable>(out var self))
            self.TakeDamage(damage);

        return;
    }

    // 2. Otherwise, apply damage to the other object if damageable
    if (other.TryGetComponent<IDamageable>(out var damageable))
    {
        damageable.TakeDamage(damage);
    }
}


}
