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

    Rigidbody2D rb;
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
        transform.localScale = new(Mathf.Sign(dir.x), 1, 1);
        rb.velocity = dir.normalized * speed;
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
        if (returning && other.transform == player) { Catch(); return; }

        if (other.TryGetComponent<IObstacle>(out var o))
        {
            if (o.Type == ObstacleType.Fire) return;
            if (o.Type == ObstacleType.Rock) o.DestroyObstacle();
        }

        if (other.TryGetComponent<IDamageable>(out var d) && !other.TryGetComponent<Fire>(out _))
            d.TakeDamage(_damage);

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
