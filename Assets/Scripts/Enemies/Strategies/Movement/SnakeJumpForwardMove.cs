using UnityEngine;

public class SnakeJumpForwardMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] float jumpInterval  = 2f;   // wait time between hops
    [SerializeField] float jumpDuration  = 0.35f;// time to complete one hop
    [SerializeField] float hopDistance   = 2f;   // horizontal travel per hop
    [SerializeField] float hopHeight     = 1f;   // peak height of the arc
    [SerializeField] Vector2 hopDir      = Vector2.left; // must be horizontal (x ≠ 0, y = 0)

    float     _nextJumpTime;
    bool      _isHopping;
    float     _t;                // 0→1 progress across the arc
    Vector3   _startPos, _endPos;

    public void Move(Transform enemyTf)
    {
        if (_isHopping)
        {
            _t += Time.deltaTime / jumpDuration;
            float parabolic = 4f * _t * (1f - _t);               // 0→1→0 curve
            Vector3 pos     = Vector3.Lerp(_startPos, _endPos, _t);
            pos.y          += hopHeight * parabolic;             // lift, then settle
            enemyTf.position = pos;

            if (_t >= 1f) _isHopping = false;                    // hop finished
            return;
        }

        if (Time.time < _nextJumpTime) return;                   // wait for next hop

        // --- start a new hop ---
        _isHopping   = true;
        _t           = 0f;
        _startPos    = enemyTf.position;
        _endPos      = _startPos + (Vector3)(hopDir.normalized * hopDistance);
        _nextJumpTime = Time.time + jumpInterval;
    }
}
