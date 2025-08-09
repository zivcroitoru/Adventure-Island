using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public sealed class ProjectileBoomerang : BaseProjectile
{
    [Header("Flight")]
    [SerializeField] float speed = 10f;
    [SerializeField] float spinSpeed = 720f;
    [SerializeField] float returnDelay = 1.2f;
    [SerializeField] float catchDist = 0.5f;

    Transform player;
    bool returning;
    bool caught;                 // guard double-catch
    public Action OnReturned;    // weapon listens: rearm, sfx, etc.

    void OnEnable()
    {
        Rb.gravityScale = 0f;
        Rb.velocity = Vector2.zero;
        returning = false;
        caught = false;
    }

    public void SetPlayer(Transform playerTransform) => player = playerTransform;

    public override void Shoot(Vector2 origin, Vector2 dir, float _ = 0f)
    {
        var n = dir.sqrMagnitude > 0f ? dir.normalized : Vector2.right;

        // Face along x sign (simple sprite flip)
        transform.localScale = new Vector3(Mathf.Sign(n.x == 0f ? 1f : n.x), 1f, 1f);
        transform.rotation = Quaternion.identity;

        base.Shoot(origin, n, speed);
        Invoke(nameof(StartReturn), returnDelay);
    }

    void StartReturn() => returning = true;

    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        if (!returning || !player || caught) return;

        Vector2 toPlayer = (player.position - transform.position);
        if (toPlayer.sqrMagnitude > 1e-4f) Rb.velocity = toPlayer.normalized * speed;
        if (toPlayer.magnitude <= catchDist) Catch();
    }

    // Override to control boomerang behavior; skip base auto-despawn path.
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (caught) return;

        // Catch by touching the player while returning
        if (returning && player && (other.transform == player || other.transform.IsChildOf(player)))
        {
            Catch();
            return;
        }

        var rootGO = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (IsSelf(rootGO)) return;

        // Special obstacle rules
        var obstacle = rootGO.GetComponentInParent<IObstacle>();
        if (obstacle != null)
        {
            if (obstacle.Type == ObstacleType.Fire)
            {
                if (!returning) StartReturn();
                return;
            }
            if (obstacle.Type == ObstacleType.Rock)
            {
                obstacle.DestroyObstacle();
                if (!returning) StartReturn();
                return;
            }
        }

        // Normal hit: deal damage and start returning (do not despawn).
        HandleHit(rootGO);
    }

    // Do damage, but keep flying; boomerang returns to player instead of despawning.
    protected override void HandleHit(GameObject hitGO)
    {
        Damage.Deal(_damage, gameObject, hitGO);
        if (!returning) StartReturn();
    }

    void Catch()
    {
        if (caught) return;
        caught = true;
        OnReturned?.Invoke();   // notify weapon first
        ReturnToPool();         // then hand to pool (pool will call OnDespawn)
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        CancelInvoke();
        OnReturned = null;      // avoid leaks / stale listeners
        player = null;
        returning = false;
        caught = false;
        // no events, no ReturnToPool() here
    }
}
