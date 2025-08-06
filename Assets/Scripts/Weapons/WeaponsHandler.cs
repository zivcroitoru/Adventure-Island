using UnityEngine;

public class WeaponsHandler : MonoBehaviour, IAttacker
{
    private IWeapon currentWeapon;
    private Animator playerAnimator;

    private void Awake()
    {
        var parent = transform.parent;
        var visual = parent?.Find("Visual");

        playerAnimator = visual?.GetComponent<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogWarning("[WeaponsHandler] Animator not found under parent/Visual.");
        }
    }

    // === EQUIP ===
    public void Equip(IWeapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("[WeaponsHandler] Tried to equip null weapon.");
            return;
        }

        ClearWeapon(); // Centralized cleanup

        currentWeapon = weapon;
        currentWeapon.Equip();

        SetWeaponActive(currentWeapon, true);
        InjectAnimator(currentWeapon);

        Debug.Log($"[WeaponsHandler] Equipped: {currentWeapon.GetType().Name}");
    }

    // === USE ===
    public void Attack()
    {
        if (CanAttack())
            currentWeapon.Attack();
        else
            Debug.Log("[WeaponsHandler] No usable weapon.");
    }

    public bool CanAttack() =>
        currentWeapon != null && currentWeapon.CanAttack();

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
        if (weapon is MonoBehaviour mb)
            mb.gameObject.SetActive(active);
    }

    private void InjectAnimator(IWeapon weapon)
    {
        if (playerAnimator != null && weapon is AnimatorAttackerBase animatedWeapon)
        {
            animatedWeapon.SetAnimator(playerAnimator);
        }
    }
}
