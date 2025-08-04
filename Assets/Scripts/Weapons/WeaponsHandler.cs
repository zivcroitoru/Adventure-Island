using UnityEngine;

public class WeaponsHandler : MonoBehaviour, IAttacker
{
    private IWeapon currentWeapon;
    private Animator playerAnimator;

private void Awake()
{
    Transform parent = transform.parent;

    if (parent == null)
    {
        Debug.LogWarning("[WeaponsHandler] No parent found.");
        return;
    }

    Debug.Log($"[WeaponsHandler] Found parent: {parent.name}");

    Transform visual = parent.Find("Visual");

    if (visual == null)
    {
        Debug.LogWarning("[WeaponsHandler] Could not find 'Visual' child under parent.");
        return;
    }

    Debug.Log($"[WeaponsHandler] Found 'Visual' object: {visual.name}");

    playerAnimator = visual.GetComponent<Animator>();

    if (playerAnimator == null)
    {
        Debug.LogWarning("[WeaponsHandler] 'Visual' object has no Animator component.");
    }
    else
    {
        Debug.Log("[WeaponsHandler] Animator successfully assigned from 'Visual'.");
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

        UnEquipCurrentWeapon();

        currentWeapon = weapon;
        currentWeapon.Equip();

        EnableWeaponObject(currentWeapon);
        InjectAnimatorIfApplicable(currentWeapon);

        Debug.Log($"[WeaponsHandler] Equipped weapon: {currentWeapon}");
    }

    // === USE ===
    public void UseWeapon()
    {
        if (CanAttack())
        {
            currentWeapon.Attack();
        }
        else
        {
            Debug.Log("[WeaponsHandler] No weapon equipped or cannot attack.");
        }
    }

    public void Attack() => UseWeapon();

    public bool CanAttack() => currentWeapon != null && currentWeapon.CanAttack();

    public void ClearWeapon()
    {
        if (currentWeapon == null) return;

        currentWeapon.UnEquip();
        DisableWeaponObject(currentWeapon);

        Debug.Log($"[WeaponsHandler] Weapon cleared: {currentWeapon}");
        currentWeapon = null;
    }

    // === Internals ===
    private void UnEquipCurrentWeapon()
    {
        if (currentWeapon == null) return;

        Debug.Log("[WeaponsHandler] Un-equipping previous weapon.");
        currentWeapon.UnEquip();
        DisableWeaponObject(currentWeapon);
    }

    private void EnableWeaponObject(IWeapon weapon)
    {
        if (weapon is MonoBehaviour mb)
            mb.gameObject.SetActive(true);
    }

    private void DisableWeaponObject(IWeapon weapon)
    {
        if (weapon is MonoBehaviour mb)
            mb.gameObject.SetActive(false);
    }

    private void InjectAnimatorIfApplicable(IWeapon weapon)
    {
        if (weapon is AnimatorAttackerBase attacker && playerAnimator != null)
        {
            attacker.SetAnimator(playerAnimator);
            Debug.Log("[WeaponsHandler] Animator injected into weapon.");
        }
    }
}
