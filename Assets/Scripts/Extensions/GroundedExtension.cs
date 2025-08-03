using UnityEngine;

public static class GroundedExtension
{
    public static bool IsGrounded(this Collider2D col, LayerMask groundLayer)
    {
        if (col == null) return false;

        Bounds bounds = col.bounds;
        Vector2 center = new(bounds.center.x, bounds.min.y - 0.05f);
        Vector2 size = new(bounds.size.x * 0.9f, 0.05f);

        Debug.DrawLine(center + Vector2.left * size.x / 2, center + Vector2.right * size.x / 2, Color.green, 0.1f);

        return Physics2D.OverlapBox(center, size, 0f, groundLayer);
    }
}
