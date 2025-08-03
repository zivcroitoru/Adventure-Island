using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D groundCheckCollider;

    private Animator animator;
    private Rigidbody2D rigid;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float speed = Mathf.Abs(rigid.velocity.x);
        float vertical = rigid.velocity.y;
        bool isGrounded = groundCheckCollider.IsGrounded(groundLayer);

        animator.SetFloat("speed", speed);
        animator.SetFloat("verticalVelocity", vertical);
        animator.SetBool("isGrounded", isGrounded);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}
