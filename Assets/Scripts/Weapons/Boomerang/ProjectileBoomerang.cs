using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public sealed class ProjectileBoomerang : BaseProjectile
{
    [Header("Flight")]
    [SerializeField] float speed       = 10f;
    [SerializeField] float spinSpeed   = 720f;
    [SerializeField] float returnDelay = 1.2f;
    [SerializeField] float catchDist   = 0.5f;

    Transform player;
    bool returning;

    public Action OnReturned;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        returning = false;
    }

    public void SetPlayer(Transform playerTransform) => player = playerTransform;

    public override void Shoot(Vector2 origin, Vector2 dir, float _ = 0f)
    {
        transform.position = origin;
        transform.localScale = new(Mathf.Sign(dir.x), 1f, 1f);
        rb.velocity = dir.normalized * speed;
        Invoke(nameof(StartReturn), returnDelay);
    }

    void StartReturn() => returning = true;

    void Update()
    {
        // Spin
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        // Home back to player when returning
        if (!returning || player == null) return;

        rb.velocity = (player.position - transform.position).normalized * speed;

        if (Vector2.Distance(transform.position, player.position) <= catchDist)
            Catch();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Catch if we touched the player or any of its children while returning
        if (returning && player != null && (other.transform == player || other.transform.IsChildOf(player)))
        {
            Catch();
            return;
        }

        // Resolve the *root* thing we hit (handles child colliders & rigidbodies)
        var target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        // Look for an obstacle on the target or its parents
        var obstacle = target.GetComponentInParent<IObstacle>();
        if (obstacle != null)
        {
            // Boomerang ignores fire
            if (obstacle.Type == ObstacleType.Fire)
            {
                if (!returning) StartReturn();
                return; // ignore
            }

            // Rocks break on boomerang hit
            if (obstacle.Type == ObstacleType.Rock)
            {
                obstacle.DestroyObstacle();

                // Optionally begin return immediately after breaking the rock
                if (!returning) StartReturn();
                return; // handled; skip base to avoid double-processing
            }
        }

        // For everything else, use the shared projectile damage logic
        base.OnTriggerEnter2D(other);

        // After first hit, start return if not already returning
        if (!returning) StartReturn();
    }

    void Catch()
    {
        OnReturned?.Invoke();
        ReturnToPool();
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        rb.velocity = Vector2.zero;
        player = null;
        returning = false;
    }
}
