using UnityEngine;

public static class GroundedExtension
{
    // More reliable IsGrounded using a box check
public static bool IsGrounded(this Transform transform, LayerMask groundLayer)
{
    Collider2D col = transform.GetComponent<Collider2D>();
    if (col == null) return false;

    Bounds bounds = col.bounds;

    // Slightly below the collider bounds
    Vector2 center = new Vector2(bounds.center.x, bounds.min.y - 0.05f);
    Vector2 size = new Vector2(bounds.size.x * 0.9f, 0.05f);

    Debug.DrawLine(center + Vector2.left * size.x / 2, center + Vector2.right * size.x / 2, Color.green, 0.1f);

    bool isGrounded = Physics2D.OverlapBox(center, size, 0f, groundLayer);
   // Debug.Log($"IsGrounded at {center} size {size}: {isGrounded}");
    return isGrounded;
}
}
