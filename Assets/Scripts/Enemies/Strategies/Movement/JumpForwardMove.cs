using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpForwardMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 jumpDirection = new Vector2(-1f, 1f);

    private Rigidbody2D _rb;
    private float _nextJumpTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Transform enemyTransform)
    {
        if (Time.time < _nextJumpTime)
            return;

        Vector2 force = jumpDirection.normalized * jumpForce;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);

        _nextJumpTime = Time.time + jumpInterval;
    }
}
