using UnityEngine;

public class WeaponsHandler : MonoBehaviour, IAttacker
{
    private IWeapon currentWeapon;
    private Transform playerRoot;

    private void Awake()
    {
        playerRoot = transform.root;
    }

    // === EQUIP ===
    public void Equip(IWeapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("[WeaponsHandler] Tried to equip null weapon.");
            return;
        }

        if (currentWeapon == weapon)
        {
            Debug.Log("[WeaponsHandler] Weapon already equipped.");
            return;
        }

        ClearWeapon();
        currentWeapon = weapon;
        currentWeapon.Equip();
        SetWeaponActive(currentWeapon, true);

        if (currentWeapon is AnimatorAttackerBase animatedWeapon)
        {
            var visual = playerRoot.Find("Visual");
            if (visual != null && visual.TryGetComponent<Animator>(out var anim))
            {
                animatedWeapon.SetAnimator(anim);
                Debug.Log("[WeaponsHandler] Animator injected.");
            }
            else
            {
                Debug.LogWarning("[WeaponsHandler] Visual/Animator not found.");
            }
        }

        Debug.Log($"[WeaponsHandler] Equipped: {currentWeapon.GetType().Name}");
    }

    // === USE ===
    public void Attack()
    {
        if (CanAttack())
        {
            currentWeapon.Attack();
        }
        else
        {
            Debug.Log("[WeaponsHandler] No usable weapon.");
        }
    }

    public bool CanAttack() =>
        currentWeapon != null && currentWeapon.CanAttack();

    // === CLEAR ===
    public void ClearWeapon()
    {
        if (currentWeapon == null) return;

        currentWeapon.UnEquip();
        SetWeaponActive(currentWeapon, false);

        Debug.Log($"[WeaponsHandler] Cleared: {currentWeapon.GetType().Name}");
        currentWeapon = null;
    }

    // === Internals ===
    private void SetWeaponActive(IWeapon weapon, bool active)
    {
        if (weapon is MonoBehaviour mb && mb != null)
        {
            mb.gameObject.SetActive(active);
        }
    }
}
