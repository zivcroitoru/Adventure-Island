using UnityEngine;

public abstract class BaseWeapon : AnimatorAttackerBase, IWeapon
{
    [Header("Weapon Settings")]
    [SerializeField] protected float shootCooldown = 0.5f;

    private float _lastShootTime;
    private bool _isEquipped;

    // Equip the weapon
    public virtual void Equip() => _isEquipped = true;

    // Unequip the weapon
    public virtual void UnEquip() => _isEquipped = false;

    // Check if equipped
    public bool IsEquipped() => _isEquipped;

    // Check if weapon can attack (based on cooldown)
   public override bool CanAttack() => Time.time >= _lastShootTime + shootCooldown;


    // Attack entry point
    public override void Attack()
    {
        if (!_isEquipped || !CanAttack())
            return;

        _lastShootTime = Time.time;
        base.Attack(); // triggers animation and OnAttack()
    }

    // Called after animation trigger; subclasses implement firing
    protected override void OnAttack() => Fire();

    // Abstract method for shooting logic
    protected abstract void Fire();
}
