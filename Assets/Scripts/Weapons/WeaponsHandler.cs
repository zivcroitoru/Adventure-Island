using UnityEngine;

public class WeaponsHandler : MonoBehaviour, IAttacker
{
    private IWeapon currentWeapon;

    // === EQUIP ===
    public void Equip(IWeapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("[WeaponsHandler] Tried to equip null weapon.");
            return;
        }

        // Un-equip current weapon if needed
        if (currentWeapon != null)
        {
            Debug.Log("[WeaponsHandler] Un-equipping previous weapon.");
            currentWeapon.UnEquip();

            if (currentWeapon is MonoBehaviour oldMb)
                oldMb.gameObject.SetActive(false);
        }

        currentWeapon = weapon;
        currentWeapon.Equip();

        if (currentWeapon is MonoBehaviour weaponMb)
            weaponMb.gameObject.SetActive(true);

        Debug.Log($"[WeaponsHandler] Equipped weapon: {currentWeapon}");
    }

    // === USE ===
    public void UseWeapon()
    {
        if (currentWeapon != null && currentWeapon.CanAttack())
        {
            currentWeapon.Attack();
        }
        else
        {
            Debug.Log("[WeaponsHandler] No weapon equipped or cannot attack.");
        }
    }

    // === IAttacker Interface ===
    public void Attack()
    {
        UseWeapon();
    }

    public bool CanAttack()
    {
        return currentWeapon != null && currentWeapon.CanAttack();
    }

    // === OPTIONAL: UnEquip ===
    public void ClearWeapon()
    {
        if (currentWeapon == null) return;

        currentWeapon.UnEquip();

        if (currentWeapon is MonoBehaviour weaponMb)
            weaponMb.gameObject.SetActive(false);

        Debug.Log($"[WeaponsHandler] Weapon cleared: {currentWeapon}");
        currentWeapon = null;
    }
}
