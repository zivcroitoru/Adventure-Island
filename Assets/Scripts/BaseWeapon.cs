using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IUseableWeapon
{
    [SerializeField] protected float shootCooldown = 0.5f;
    protected float _lastShootTime;

    protected bool _isEquipped = false;

    public virtual void Equip() => _isEquipped = true;
    public virtual void UnEquip() => _isEquipped = false;
    public bool IsEquipped() => _isEquipped;

    public void Shoot()
    {
        if (!_isEquipped || Time.time < _lastShootTime + shootCooldown)
            return;

        _lastShootTime = Time.time;
        Fire();
    }

    // Subclasses must implement this
    protected abstract void Fire();
}
