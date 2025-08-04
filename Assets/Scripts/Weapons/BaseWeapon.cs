using UnityEngine;

/// <summary>
/// Abstract weapon base that handles equip state, cooldown, animation triggering, and firing.
/// </summary>
public abstract class BaseWeapon : AnimatorAttackerBase, IWeapon
{
    [Header("Weapon Settings")]
    [SerializeField] protected float shootCooldown = 0.5f;

    protected float lastShootTime;
    protected bool isEquipped = false;

    // === IWeapon ===
    public virtual void Equip() => isEquipped = true;
    public virtual void UnEquip() => isEquipped = false;
    public bool IsEquipped() => isEquipped;

    public virtual bool CanAttack() => Time.time >= lastShootTime + shootCooldown;

    public override void Attack()
    {
        if (!isEquipped || !CanAttack()) return;

        lastShootTime = Time.time;
        base.Attack(); // triggers animation + OnAttack
    }

    /// <summary>
    /// Called after attack animation trigger.
    /// </summary>
    protected override void OnAttack()
    {
        Fire();
    }

    /// <summary>
    /// Subclasses implement actual shooting logic here.
    /// </summary>
    protected abstract void Fire();
}
