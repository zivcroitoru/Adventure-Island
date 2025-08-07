using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkDistance = 0.1f;

    private Collider2D col;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (col == null) return;

        // Use collider bounds bottom center to check for ground
        Vector2 origin = col.bounds.center;
        origin.y = col.bounds.min.y;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayer);
        IsGrounded = hit.collider != null;
    }
}
