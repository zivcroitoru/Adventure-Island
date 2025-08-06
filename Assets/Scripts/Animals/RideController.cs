using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RideController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;

    public static event System.Action<GameObject> OnAnimalMounted;
    public static event System.Action<GameObject> OnAnimalDismounted;

    private Rigidbody2D rb;
    private WeaponsHandler weaponHandler;
    private AnimalBase currentAnimal;

    // === Public Properties ===
    public AnimalBase CurrentAnimal => currentAnimal;
    public bool IsRiding => currentAnimal != null;
    public IAttacker CurrentAttacker => IsRiding ? currentAnimal : weaponHandler;

    // === Init ===
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponHandler = GetComponentInChildren<WeaponsHandler>();
    }

    // === Mount System ===
    public bool SwitchAnimal(AnimalBase newAnimal)
    {
        if (newAnimal == null) return false;

        newAnimal.transform.SetParent(null); // avoid accidental destroy
        DismountCurrentAnimal();

        currentAnimal = newAnimal;
        currentAnimal.Mount(gameObject);

        ApplyAnimatorOverride(currentAnimal.GetOverrideController());
        currentAnimal.SetAnimator(animator);

        OnAnimalMounted?.Invoke(gameObject);
        return true;
    }

    public void DismountCurrentAnimal()
    {
        if (!IsRiding) return;

        currentAnimal.Dismount();
        currentAnimal = null;

        ResetAnimatorToBase();
        OnAnimalDismounted?.Invoke(gameObject);
    }

    public void UnmountAnimal()
    {
        if (IsRiding)
            DismountCurrentAnimal();
    }

    private void ApplyAnimatorOverride(RuntimeAnimatorController overrideCtrl)
    {
        animator.runtimeAnimatorController = overrideCtrl ?? baseController;
    }

    public void ResetAnimatorToBase()
    {
        ApplyAnimatorOverride(baseController);
    }

    // === Combat ===
    public void UseAttack()
    {
        if (CurrentAttacker?.CanAttack() == true)
            CurrentAttacker.Attack();
    }

    // === Obstacle Logic ===
    public bool TryDestroyObstacle(ObstacleType type)
    {
        if (!IsRiding) return false;
        return currentAnimal.CanDestroy(type);
    }

    // === Weapon System ===
    public void EquipWeapon(IWeapon weapon)
    {
        weaponHandler?.Equip(weapon);
    }
}
