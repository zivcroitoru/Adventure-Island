using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public sealed class ProjectileBoomerang : BaseProjectile
{
    [SerializeField] float speed = 10f, spinSpeed = 720f, returnDelay = 1.2f, catchDist = 0.5f;

    Transform player;
    bool returning, caught;
    public Action OnReturned;

    void OnEnable() { Rb.gravityScale = 0f; Rb.velocity = Vector2.zero; returning = caught = false; }
    public void SetPlayer(Transform t) => player = t;

    public override void Shoot(Vector2 origin, Vector2 dir, float _ = 0f)
    {
        var n = dir.sqrMagnitude > 0f ? dir.normalized : Vector2.right;
        transform.localScale = new Vector3(Mathf.Sign(n.x == 0f ? 1f : n.x), 1f, 1f);
        transform.rotation = Quaternion.identity;
        base.Shoot(origin, n, speed);
        Invoke(nameof(StartReturn), returnDelay);
    }

    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        if (!returning || !player || caught) return;
        var to = (player.position - transform.position);
        if (to.sqrMagnitude > 1e-4f) Rb.velocity = to.normalized * speed;
        if (to.magnitude <= catchDist) Catch();
    }

protected override void OnTriggerEnter2D(Collider2D other)
{
    if (caught) return;

    // Catch on player while returning
    if (returning && player && (other.transform == player || other.transform.IsChildOf(player)))
    {
        Catch();
        return;
    }

    var hit = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
    if (IsSelf(hit)) return;

    // Check for obstacle
    if (hit.TryGetComponent<IObstacle>(out var obs) ||
        (obs = hit.GetComponentInParent<IObstacle>()) != null)
    {
        // Skip if Fire obstacle OR Ghost enemy
        if (obs.Type == ObstacleType.Fire ||
            (hit.GetComponentInParent<EnemyBase>()?.EnemyType == EnemyType.Ghost))
        {
            if (!returning) StartReturn();
            return;
        }

        // Break obstacle
        obs.DestroyObstacle();
        if (!returning) StartReturn();
        return;
    }

    // Not an obstacle → deal damage
    HandleHit(hit);
}


    protected override void HandleHit(GameObject hit)
    {
        Damage.Deal(_damage, gameObject, hit);
        if (!returning) StartReturn();
    }

    void StartReturn() => returning = true;

    void Catch()
    {
        if (caught) return;
        caught = true;
        OnReturned?.Invoke();
        ReturnToPool();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        CancelInvoke();
        OnReturned = null;
        player = null;
        returning = caught = false;
    }
}
