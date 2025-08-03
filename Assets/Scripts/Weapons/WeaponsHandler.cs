using UnityEngine;

public class WeaponsHandler : AnimatorAttackerBase
{
    public AxeWeapon axeWeapon;
    public BoomerangWeapon boomerangWeapon;

    [SerializeField] private float attackCooldown = 0.5f; // seconds
    private float lastAttackTime;

    private void Awake()
    {
        var parent = transform.parent;
        if (parent != null)
        {
            var animator = parent.GetComponentInChildren<Animator>();
            if (animator != null)
                SetAnimator(animator);
        }

        if (string.IsNullOrEmpty(triggerName))
            triggerName = "Shoot";
    }

    public override bool CanAttack()
    {
        bool weaponReady = axeWeapon?.IsEquipped() == true || boomerangWeapon?.IsEquipped() == true;
        bool cooldownReady = Time.time >= lastAttackTime + attackCooldown;
        return weaponReady && cooldownReady;
    }

    protected override void OnAttack()
    {
        lastAttackTime = Time.time;

        if (axeWeapon?.IsEquipped() == true)
        {
            axeWeapon.Shoot();
            Debug.Log("[WeaponsHandler] Axe attack.");
        }
        else if (boomerangWeapon?.IsEquipped() == true)
        {
            boomerangWeapon.Shoot();
            Debug.Log("[WeaponsHandler] Boomerang attack.");
        }
    }
    public void Equip(BaseWeapon weapon)
{
    // Unequip all
    axeWeapon?.UnEquip();
    boomerangWeapon?.UnEquip();

    // Equip selected
    weapon?.Equip();

    Debug.Log($"[WeaponsHandler] Equipped: {weapon?.GetType().Name}");
}

}
