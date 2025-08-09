using UnityEngine;

public sealed class ProjectileSpark : BaseProjectile
{
    [SerializeField] float lifetime = 0.3f;
    [SerializeField] float flickerInterval = 0.05f;
    bool _flipState;

    public override void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f)
    {
        var n = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
        transform.rotation = Quaternion.identity;
        base.Shoot(origin, n, _speed);
        _flipState = n.x < 0f;
        if (Sr) Sr.flipX = _flipState;
        InvokeRepeating(nameof(ToggleFlip), 0f, flickerInterval);
        Invoke(nameof(ReturnToPool), lifetime);
    }

    void ToggleFlip() { _flipState = !_flipState; if (Sr) Sr.flipX = _flipState; }

protected override void OnTriggerEnter2D(Collider2D other)
{
    base.OnTriggerEnter2D(other); // keep base behavior if needed

    var hit = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
    if (!hit || (ownerRoot && hit.transform.root == ownerRoot))
        return;

    // Check for obstacle
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

        // Otherwise destroy it
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
        CancelInvoke(nameof(ToggleFlip));
        CancelInvoke();
        _flipState = false;
        if (Sr) Sr.flipX = false;
    }
}
