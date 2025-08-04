using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpForwardMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 jumpDirection = new Vector2(-1f, 1f); // X = forward, Y = upward

    private Rigidbody2D _rb;
    private float _nextJumpTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Debug.Log("[JumpForwardMove] Awake - Rigidbody2D assigned.");
    }

    public void Move(Transform enemyTransform)
    {
        if (Time.time < _nextJumpTime)
        {
            Debug.Log($"[JumpForwardMove] Not time to jump yet. Next jump at {_nextJumpTime:F2}, current time: {Time.time:F2}");
            return;
        }

        Debug.Log($"[JumpForwardMove] Jumping at time {Time.time:F2}");

        Vector2 force = jumpDirection.normalized * jumpForce;
        Debug.Log($"[JumpForwardMove] Applying force: {force}");

        _rb.velocity = Vector2.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);

        _nextJumpTime = Time.time + jumpInterval;
        Debug.Log($"[JumpForwardMove] Next jump scheduled at {_nextJumpTime:F2}");
    }
}
