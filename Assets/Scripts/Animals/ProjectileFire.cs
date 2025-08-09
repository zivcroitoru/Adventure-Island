using UnityEngine;

public sealed class ProjectileFire : BaseProjectile
{
    [SerializeField] float lifetime = 3f;
    [SerializeField] float spawnOffset = 0.3f;

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        var n = dir.sqrMagnitude > 0f ? dir.normalized : Vector2.right;
        Vector2 offset = n * spawnOffset + Vector2.down * 0.2f;
        float forwardBonus = Mathf.Max(0f, n.x * playerSpeed);

        base.Shoot(origin + offset, n, _speed + forwardBonus);

        transform.rotation = Quaternion.identity;
        if (Sr) Sr.flipX = n.x < 0f;

        Invoke(nameof(ReturnToPool), lifetime);
    }

protected override void OnTriggerEnter2D(Collider2D other)
{
    base.OnTriggerEnter2D(other);

    var hit = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
    if (!hit || (ownerRoot && hit.transform.root == ownerRoot))
        return; // ignore self/owner

    // Check if hit is an obstacle (enemies may also implement IObstacle)
    if (hit.TryGetComponent<IObstacle>(out var obs) ||
        (obs = hit.GetComponentInParent<IObstacle>()) != null)
    {
        // Skip if Fire obstacle OR Ghost enemy
        if (obs.Type == ObstacleType.Fire ||
            (hit.GetComponentInParent<EnemyBase>()?.EnemyType == EnemyType.Ghost))
        {
            ReturnToPool();
            return;
        }

        // Otherwise, break the obstacle
        obs.DestroyObstacle();
        ReturnToPool();
        return;
    }

    // Not an obstacle → try regular damage
    if (Damage.Deal(_damage, gameObject, hit))
        ReturnToPool();
}
    public override void OnDespawn()
    {
        base.OnDespawn();
        CancelInvoke();
    }
}
