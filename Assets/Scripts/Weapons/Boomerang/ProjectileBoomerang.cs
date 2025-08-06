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

    private Rigidbody2D rb;
    private Transform player;
    private bool returning;

    public Action OnReturned;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable()
    {
        rb.gravityScale = 0f;
        rb.velocity     = Vector2.zero;
        returning       = false;
    }

    public override void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f)
    {
        // Fallback, not used
    }

    public void Shoot(Transform playerTransform, Vector2 direction)
    {
        player = playerTransform;

        transform.position   = player.position;
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1f, 1f);
        rb.velocity          = direction.normalized * speed;

        Invoke(nameof(StartReturn), returnDelay);
    }

    void StartReturn() => returning = true;

    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

        if (!returning || player == null) return;

        rb.velocity = (player.position - transform.position).normalized * speed;

        if (Vector2.Distance(transform.position, player.position) <= catchDist)
            Catch();
    }

protected override void OnTriggerEnter2D(Collider2D other)
{
    // Catch boomerang if it hits the player during return
    if (returning && other.transform == player)
    {
        Catch();
        return;
    }

    // Handle obstacles
    if (other.TryGetComponent<IObstacle>(out var obstacle))
    {
        if (obstacle.Type == ObstacleType.Fire)
            return; // Skip fire completely

        if (obstacle.Type == ObstacleType.Rock)
            obstacle.DestroyObstacle(); // If you want to break rocks ONLY
    }

    // Deal damage if it's damageable (excluding fire)
    if (other.TryGetComponent<IDamageable>(out var dmg) && 
        (!other.TryGetComponent<Fire>(out _))) // Exclude fire
    {
        dmg.TakeDamage(_damage);
    }

    // Always trigger return if it hit something (except fire)
    if (!returning)
        StartReturn();
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
        player      = null;
        returning   = false;
    }
}
