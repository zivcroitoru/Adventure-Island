using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FrogJumpAttackMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private Vector2 jumpDirection = new Vector2(-2f, 1.5f);
    [SerializeField] private float postLandDelay = 3f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private string jumpBoolParam = "IsJumping";

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _nextJumpTime;
    private bool _wasGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void Move(Transform enemyTransform)
    {
        bool isGrounded = CheckGrounded();

        HandleLanding(isGrounded);
        _wasGrounded = isGrounded;

        if (!CanJump(isGrounded, enemyTransform.position))
            return;

        StartJump();
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void HandleLanding(bool isGrounded)
    {
        if (!_wasGrounded && isGrounded && _rb.velocity.y <= 0.1f)
        {
            _animator.SetBool(jumpBoolParam, false);
            _rb.velocity = Vector2.zero;
            _nextJumpTime = Time.time + postLandDelay;
        }
    }

    private bool CanJump(bool isGrounded, Vector2 enemyPosition)
    {
        return isGrounded
            && Time.time >= _nextJumpTime
            && IsPlayerNearby(enemyPosition);
    }

    private void StartJump()
    {
        _animator.SetBool(jumpBoolParam, true);

        Vector2 force = jumpDirection.normalized * jumpForce;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);

        _nextJumpTime = float.MaxValue; // Locked until landing
    }

    private bool IsPlayerNearby(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, detectionRadius, playerLayer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckDistance);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}
