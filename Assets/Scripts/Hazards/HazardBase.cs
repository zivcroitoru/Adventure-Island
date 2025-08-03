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
        if (!other.TryGetComponent(out IDamageable damageable)) return;
        if (other.TryGetComponent(out IInvincible invincible) && invincible.IsInvincible) return;

        damageable.TakeDamage(damage);
    }
}
