using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 100f;

    [Header("Ground Check")]
    [SerializeField] private GroundCheckProvider groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var groundCollider = groundCheck?.CurrentGroundCollider;
        if (groundCollider == null) return;

        bool isGrounded = groundCollider.IsGrounded(groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        rigid.AddForce(Vector2.up * jumpSpeed);
        animator?.SetTrigger("Jump");
    }
}
