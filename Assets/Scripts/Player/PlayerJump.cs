using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpSpeed = 100;           // Force applied when jumping
    public LayerMask groundLayer;           // What counts as ground

    private Rigidbody2D rigid;
    private Animator animator;
    private bool canDoubleJump = true;      // Allows one mid-air jump

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Jump()
    {
        // Cancel any vertical velocity before jump (for consistent height)
        rigid.velocity = new Vector2(rigid.velocity.x, 0);

        // Add upward force to make the player jump
        rigid.AddForce(Vector2.up * jumpSpeed);

        // Trigger jump animation if available
        if (animator != null)
            animator.SetTrigger("Jump");
    }

    void Update()
    {
        // Check if the player is standing on the ground
        bool isGrounded = transform.IsGrounded(groundLayer);

        // Reset double jump when touching the ground
        if (isGrounded)
            canDoubleJump = true;

        // When space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // Normal jump from the ground
                Jump();
            }
            else if (canDoubleJump)
            {
                // If in air and double jump is available, jump again
                Jump();
                canDoubleJump = false; // Disable further double jumps
            }
        }
    }
}
