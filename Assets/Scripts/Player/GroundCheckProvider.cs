using UnityEngine;

public class GroundCheckProvider : MonoBehaviour
{
    [Tooltip("Currently active collider used to check if grounded.")]
    public Collider2D CurrentGroundCollider { get; private set; }

    /// <summary>
    /// Sets the current ground check collider (called from RideController).
    /// </summary>
    public void SetCollider(Collider2D col)
    {
        if (col == null)
        {
            Debug.LogWarning("[GroundCheckProvider] SetCollider called with null.");
            return;
        }

        CurrentGroundCollider = col;
        Debug.Log($"[GroundCheckProvider] Active collider set to: {col.name}");
    }
}
