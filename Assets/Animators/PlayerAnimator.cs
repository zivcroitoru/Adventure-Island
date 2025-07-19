using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator; // ← this was missing in your code
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
        bool isGrounded = transform.IsGrounded(LayerMask.GetMask("Default"));

        animator.SetFloat("speed", speed);
        animator.SetFloat("verticalVelocity", vertical);
        animator.SetBool("isGrounded", isGrounded);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump"); // ← this works now because animator is defined
    }
}
