using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RideController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;

    private Rigidbody2D rb;
    private WeaponsHandler weaponHandler;
    private AnimalBase currentAnimal;

    // === Public Properties ===
    public AnimalBase CurrentAnimal => currentAnimal;
    public bool IsRiding => currentAnimal != null;

    public IAttacker CurrentAttacker =>
        currentAnimal != null ? (IAttacker)currentAnimal : weaponHandler;

    // === Init ===
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponHandler = GetComponentInChildren<WeaponsHandler>();
    }

    // === Mounting ===
    public bool SwitchAnimal(AnimalBase newAnimal)
    {
        if (newAnimal == null)
        {
            Debug.LogError("[RideController] Tried to mount a null animal.");
            return false;
        }

        Debug.Log($"[RideController] Switching to animal: {newAnimal.name}");

        newAnimal.transform.SetParent(null); // Detach to avoid accidental destruction
        DismountCurrentAnimal();

        currentAnimal = newAnimal;
        currentAnimal.Mount(gameObject);

        ApplyAnimatorOverride(currentAnimal.GetOverrideController());
        currentAnimal.SetAnimator(animator);

        return true;
    }

    public void UnmountAnimal()
    {
        if (!IsRiding) return;

        Debug.Log("[RideController] Unmounting animal...");
        DismountCurrentAnimal();
    }

public void DismountCurrentAnimal()
{
    if (!IsRiding) return;

    Debug.Log("[RideController] Dismounting animal.");
    currentAnimal.Dismount();
    currentAnimal = null;

    ResetAnimatorToBase();
}


    public void ResetAnimatorToBase()
    {
        ApplyAnimatorOverride(baseController);
    }

    private void ApplyAnimatorOverride(RuntimeAnimatorController overrideCtrl)
    {
        animator.runtimeAnimatorController = overrideCtrl ?? baseController;

        Debug.Log(overrideCtrl != null
            ? "[RideController] OverrideController applied."
            : "[RideController] Using base animator.");
    }

    // === Combat ===
    public void UseAttack()
    {
        if (CurrentAttacker?.CanAttack() == true)
        {
            CurrentAttacker.Attack();
        }
        else
        {
            Debug.Log("[RideController] No valid attacker or cannot attack.");
        }
    }

    public bool TryDestroyObstacle(ObstacleType type)
    {
        if (!IsRiding)
        {
            Debug.LogWarning("[RideController] No animal to check obstacle destruction.");
            return false;
        }

        bool canDestroy = currentAnimal.CanDestroy(type);
        Debug.Log($"[RideController] Can destroy {type}: {canDestroy}");
        return canDestroy;
    }

    // === Weapons ===
    public void EquipWeapon(IWeapon weapon)
    {
        if (weaponHandler == null)
        {
            Debug.LogWarning("[RideController] No weapon handler found!");
            return;
        }

        weaponHandler.Equip(weapon);
        Debug.Log($"[RideController] Weapon equipped: {weapon}");
    }
}
