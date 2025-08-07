using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rigid;
    private GroundCheck groundCheck;
    private Animator animator;

    private float direction;
    public bool IsGrounded => groundCheck.IsGrounded;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("isGrounded", IsGrounded);

        direction = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);

        animator.SetFloat("speed", Mathf.Abs(direction * speed));

        if (direction != 0)
        {
            float scaleX = direction > 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
        }
    }
}
