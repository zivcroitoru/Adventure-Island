using UnityEngine;

public class FrogJumpAttackMove : MonoBehaviour, IMovementStrategy
{
    [Header("Jump Settings")]
    [SerializeField] float jumpDistance   = 4f;   // horizontal travel
    [SerializeField] float jumpHeight     = 2f;   // peak height
    [SerializeField] float jumpDuration   = 0.45f;
    [SerializeField] float postLandDelay  = 3f;
    [SerializeField] Vector2 jumpDir      = new( -1f, 0f );   // flat, will add vertical arc

    [Header("Player Detection")]
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask playerLayer;

    [Header("Animation")]
    [SerializeField] string jumpBoolParam = "IsJumping";

    Animator _anim;
    bool     _isJumping;
    float    _t;              // 0-1 progress across arc
    float    _nextJumpTime;
    Vector3  _startPos, _endPos;

    void Awake() => _anim = GetComponent<Animator>();

    public void Move( Transform enemyTf )
    {
        // Active hop: update position along arc ---------------------------------
        if ( _isJumping )
        {
            _t += Time.deltaTime / jumpDuration;
            float arc = 4f * _t * ( 1f - _t );                // 0→1→0 parabola
            enemyTf.position = Vector3.Lerp( _startPos, _endPos, _t ) +
                               Vector3.up * ( arc * jumpHeight );

            if ( _t >= 1f )
            {
                _isJumping   = false;
                _anim.SetBool( jumpBoolParam, false );
                _nextJumpTime = Time.time + postLandDelay;
            }
            return;
        }

        // Idle: decide whether to start a new hop --------------------------------
        if ( Time.time < _nextJumpTime ) return;
        if ( !Physics2D.OverlapCircle( enemyTf.position, detectionRadius, playerLayer ) ) return;

        // Begin hop --------------------------------------------------------------
        _isJumping = true;
        _t         = 0f;
        _startPos  = enemyTf.position;
        _endPos    = _startPos + ( Vector3 )( jumpDir.normalized * jumpDistance );
        _anim.SetBool( jumpBoolParam, true );
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, detectionRadius );
    }
#endif
}
