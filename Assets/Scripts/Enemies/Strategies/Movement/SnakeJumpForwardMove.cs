using UnityEngine;

/// <summary>
/// Makes the snake hop forward in a fixed arc at regular intervals.
/// </summary>
[DisallowMultipleComponent]
public class SnakeJumpForwardMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float jumpDuration = 0.35f;
    [SerializeField] private float hopDistance  = 2f;
    [SerializeField] private float hopHeight    = 1f;
    [SerializeField] private Vector2 hopDir     = Vector2.left;

    private float   groundY;
    private float   nextJumpTime;
    private bool    isHopping;
    private float   t;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Awake()
    {
        groundY = transform.position.y;
    }

    public void Move(Transform enemyTf)
    {
        if (isHopping)
        {
            UpdateHop(enemyTf);
            return;
        }

        if (Time.time >= nextJumpTime)
        {
            StartJump(enemyTf);
        }
    }

    private void StartJump(Transform enemyTf)
    {
        isHopping    = true;
        t            = 0f;
        startPos     = new Vector3(enemyTf.position.x, groundY, enemyTf.position.z);
        endPos       = startPos + (Vector3)(hopDir.normalized * hopDistance);
        nextJumpTime = Time.time + jumpInterval;
    }

    private void UpdateHop(Transform enemyTf)
    {
        t += Time.deltaTime / jumpDuration;
        float arc = 4f * t * (1f - t); // Parabolic curve
        Vector3 pos = Vector3.Lerp(startPos, endPos, t);
        pos.y = groundY + arc * hopHeight;
        enemyTf.position = pos;

        if (t >= 1f)
        {
            isHopping = false;
        }
    }
}
