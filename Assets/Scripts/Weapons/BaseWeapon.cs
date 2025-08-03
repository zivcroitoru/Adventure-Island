using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected float shootCooldown = 0.5f;
    protected float _lastShootTime;

    protected bool _isEquipped = false;

    public virtual void Equip() => _isEquipped = true;
    public virtual void UnEquip() => _isEquipped = false;
    public bool IsEquipped() => _isEquipped;

    public void Attack()
    {
        if (!_isEquipped || !CanAttack()) return;

        _lastShootTime = Time.time;
        Fire();
    }

    public virtual bool CanAttack()
    {
        return Time.time >= _lastShootTime + shootCooldown;
    }

    protected abstract void Fire();
}
