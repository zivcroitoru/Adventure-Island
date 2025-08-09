// RideController.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class RideController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;

    [Header("Dismount")]
    [SerializeField] private float dismountInvincibleSeconds = 0.5f;

    public static event System.Action<GameObject> OnAnimalMounted;
    public static event System.Action<GameObject> OnAnimalDismounted;

    private Rigidbody2D rb;
    private WeaponsHandler weaponHandler;
    private AnimalBase currentAnimal;

    // Public API
    public AnimalBase CurrentAnimal => currentAnimal;
    public bool IsRiding => currentAnimal != null;
    public IAttacker CurrentAttacker => IsRiding ? currentAnimal : weaponHandler;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponHandler = GetComponentInChildren<WeaponsHandler>(true);
        if (!animator) animator = GetComponentInChildren<Animator>(true);
    }

    // === Mount System ===
    public bool SwitchAnimal(AnimalBase newAnimal)
    {
        if (!newAnimal) return false;

        newAnimal.transform.SetParent(null);    // avoid accidental destroy with parent
        DismountCurrentAnimal();

        currentAnimal = newAnimal;
        currentAnimal.Mount(gameObject);

        // Animator override & injection
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

        // Brief invulnerability after dismount
        GrantTempInvincibility(dismountInvincibleSeconds);

        // Also suppress ramming during that temp window
        GetComponent<InvincibleRammer>()?.SuppressFor(dismountInvincibleSeconds);
    }


    public void UnmountAnimal()
    {
        if (IsRiding) DismountCurrentAnimal();
    }

    // === Combat ===
    public void UseAttack()
    {
        if (CurrentAttacker?.CanAttack() == true)
            CurrentAttacker.Attack();
    }

    // === Obstacles ===
    public bool TryDestroyObstacle(ObstacleType type)
    {
        if (!IsRiding) return false;
        return currentAnimal.CanDestroy(type);
    }

    // === Weapons ===
    public void EquipWeapon(IWeapon weapon) => weaponHandler?.Equip(weapon);

    // === Animator helpers ===
    private void ApplyAnimatorOverride(RuntimeAnimatorController overrideCtrl)
    {
        if (animator) animator.runtimeAnimatorController = overrideCtrl ? overrideCtrl : baseController;
    }

    public void ResetAnimatorToBase() => ApplyAnimatorOverride(baseController);

    // === Invincibility glue ===
    private void GrantTempInvincibility(float seconds)
    {
        // Prefer FairyInvinciblePowerUp if present (plays nice with full fairy power-up)
        var fairy = GetComponentInChildren<FairyInvinciblePowerUp>(true);
        if (fairy)
        {
            fairy.SetTemporaryInvincibility(true);
            StartCoroutine(ClearFairyAfter(fairy, seconds));
            return;
        }

        // Fallback: local helper that only provides temporary invincibility
        var helper = GetComponent<TempInvincibleHelper>();
        if (!helper) helper = gameObject.AddComponent<TempInvincibleHelper>();
        helper.Trigger(seconds);
    }

    private IEnumerator ClearFairyAfter(FairyInvinciblePowerUp fairy, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Don’t cancel if a full fairy buff is active
        if (fairy && fairy.IsTempActive && !fairy.IsPowerUpActive)
            fairy.SetTemporaryInvincibility(false);
    }


    // Minimal helper used only when no FairyInvinciblePowerUp exists.
    // Implements IInvincible so your Damage gate “just works”.
    [DisallowMultipleComponent]
    sealed class TempInvincibleHelper : MonoBehaviour, IInvincible
    {
        bool _temp;
        public bool IsInvincible => _temp;
        public void SetTemporaryInvincibility(bool state) => _temp = state;

        public void Trigger(float seconds)
        {
            StopAllCoroutines();
            StartCoroutine(Co(seconds));
        }

        System.Collections.IEnumerator Co(float s)
        {
            _temp = true;
            yield return new WaitForSeconds(s);
            _temp = false;
        }
    }
}
