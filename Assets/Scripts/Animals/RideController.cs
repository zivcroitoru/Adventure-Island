using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RideController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;

    [Header("Colliders")]
    [SerializeField] private Collider2D unmountedCollider;
    [SerializeField] private Collider2D mountedCollider;

    [Header("Ground Check")]
    [SerializeField] private GroundCheckProvider groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private AnimalBase currentAnimal;
    private WeaponsHandler weaponHandler;

    public AnimalBase CurrentAnimal => currentAnimal;

    public IAttacker CurrentAttacker =>
        currentAnimal != null ? (IAttacker)currentAnimal : weaponHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponHandler = GetComponentInChildren<WeaponsHandler>();
    }

    private void FixedUpdate()
    {
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        var activeCollider = groundCheck?.CurrentGroundCollider;
        if (activeCollider == null) return;

        bool isGrounded = activeCollider.IsGrounded(groundLayer);
        float horizontalSpeed = Mathf.Abs(rb.velocity.x);

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("speed", horizontalSpeed);
    }

    public void SwitchAnimal(AnimalBase newAnimal)
    {
        if (newAnimal == null)
        {
            Debug.LogError("[RideController] Attempted to switch to null animal.");
            return;
        }

        Debug.Log($"[RideController] Switching to animal: {newAnimal.name}");

        DismountCurrentAnimal();

        currentAnimal = newAnimal;
        currentAnimal.Mount(gameObject);

        ToggleColliders(true);

        // Apply animator override
        var overrideCtrl = currentAnimal.GetOverrideController();
        animator.runtimeAnimatorController = overrideCtrl ?? baseController;

        // 👇 Inject animator into animal so it can trigger attacks
        currentAnimal.SetAnimator(animator);

        Debug.Log(overrideCtrl != null
            ? "[RideController] OverrideController applied."
            : "[RideController] Using base animator.");
    }

    public void UnmountAnimal()
    {
        Debug.Log("[RideController] Unmounting animal...");
        DismountCurrentAnimal();
        ToggleColliders(false);
        animator.runtimeAnimatorController = baseController;
        Debug.Log("[RideController] Animator reverted to base.");
    }

    private void DismountCurrentAnimal()
    {
        if (currentAnimal == null) return;

        currentAnimal.Dismount();
        Destroy(currentAnimal.gameObject);
        currentAnimal = null;
    }

    private void ToggleColliders(bool mounted)
    {
        if (mountedCollider) mountedCollider.enabled = mounted;
        if (unmountedCollider) unmountedCollider.enabled = !mounted;

        if (groundCheck != null)
        {
            var active = mounted
                ? currentAnimal?.GetGroundCheckCollider()
                : unmountedCollider;

            groundCheck.SetCollider(active);
        }
    }

    public void UseAttack()
    {
        if (CurrentAttacker != null && CurrentAttacker.CanAttack())
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
        if (currentAnimal == null)
        {
            Debug.LogWarning("[RideController] No animal to check obstacle destruction.");
            return false;
        }

        bool canDestroy = currentAnimal.CanDestroy(type);
        Debug.Log($"[RideController] Can destroy {type}: {canDestroy}");
        return canDestroy;
    }
}
