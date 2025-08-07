using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 100f;

    private Rigidbody2D rigid;
    private GroundCheck groundCheck;
    private Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck.IsGrounded)
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
