using UnityEngine;

public sealed class ProjectileFire : BaseProjectile
{
    [SerializeField] private float lifetime    = 3f;
    [SerializeField] private float spawnOffset = 0.3f;

    // Shoots with a small forward offset; adds speed if shooter moves forward.
    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        var n = dir.sqrMagnitude > 0f ? dir.normalized : Vector2.right;

        // Offset slightly forward + down to avoid immediate self-collision
        Vector2 offset = n * spawnOffset + Vector2.down * 0.2f;

        // Bonus only when moving in the shot’s forward X direction
        float forwardBonus = Mathf.Max(0f, n.x * playerSpeed);

        // Position + velocity via base (uses lazy-cached Rb)
        base.Shoot(origin + offset, n, _speed + forwardBonus);

        // Visuals
        transform.rotation = Quaternion.identity;
        if (Sr) Sr.flipX = n.x < 0f;

        // Lifetime
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public override void OnDespawn()
    {
        base.OnDespawn(); // zeros velocity
        CancelInvoke();   // stop lifetime timer if pooled early
    }
}
