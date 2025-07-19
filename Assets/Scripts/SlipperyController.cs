using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlippyController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float slippy = 10f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Manual gravity
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(inputX * moveSpeed, 0);

        if (IsGrounded(out RaycastHit2D hit)) // Use your extension here
        {
            Vector2 slopeDir = Vector2.Perpendicular(hit.normal).normalized;
            if (slopeDir.y < 0) slopeDir *= -1;

            float slopeInfluence = Vector2.Dot(Vector2.down, hit.normal);
            velocity += slopeDir * slopeInfluence * gravity * slippy * Time.deltaTime;

            velocity.x = move.x;
            velocity *= 0.95f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        rb.velocity = velocity;
    }

    // If you don’t already return hit in your extension, you can remove this
    private bool IsGrounded(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        return hit.collider != null;
    }
}
