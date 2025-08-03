using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    [Header("Ground Check")]
    [SerializeField] private GroundCheckProvider groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private Animator animator;

    private float direction;
    public bool IsGrounded { get; private set; }

void Start()
{
    var col = GetComponent<Collider2D>();
    if (col != null)
    {
        groundCheck?.SetCollider(col);
        Debug.Log("[PlayerMovement] Assigned default player collider to GroundCheckProvider.");
    }
}
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        Debug.Log("[PlayerMovement] Awake called. Rigidbody and Animator assigned.");
    }

    void FixedUpdate()
    {
        var groundCollider = groundCheck?.CurrentGroundCollider;

        if (groundCollider == null)
        {
            Debug.LogWarning("[PlayerMovement] No ground collider set. Skipping movement.");
            return;
        }

        Debug.Log($"[PlayerMovement] Ground collider detected: {groundCollider.name}");

        IsGrounded = groundCollider.IsGrounded(groundLayer);
        Debug.Log($"[PlayerMovement] IsGrounded = {IsGrounded}");

        animator.SetBool("isGrounded", IsGrounded);

        direction = Input.GetAxisRaw("Horizontal");
        Debug.Log($"[PlayerMovement] Input direction: {direction}");

        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
        Debug.Log($"[PlayerMovement] Velocity set to: {rigid.velocity}");

        animator.SetFloat("speed", Mathf.Abs(direction * speed));

        if (direction != 0)
        {
            float scaleX = direction > 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
            Debug.Log($"[PlayerMovement] Facing direction set to: {scaleX}");
        }
    }
}
