using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class RedAnimal : AnimalBase
{
    [Header("Fire-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;

    private ProjectileFirePool _firePool;

    [Inject]
    public void Construct(ProjectileFirePool firePool)
    {
        _firePool = firePool;
    }

    protected override void OnAttack()
    {
        if (_firePool == null)
        {
            Debug.LogWarning("[RedAnimal] ❌ Fire pool not injected.");
            return;
        }

        Vector2 direction = GetFacingDirection();
        Vector2 spawnPos = transform.position + (Vector3)(direction * projectileOffset);

        var fire = _firePool.Get(spawnPos, Quaternion.identity);
        if (fire == null)
        {
            Debug.LogWarning("[RedAnimal] ❌ Failed to retrieve fire projectile.");
            return;
        }

        float playerSpeed = GetPlayerSpeed();
        fire.Shoot(spawnPos, direction, playerSpeed);

        Debug.Log($"[RedAnimal] 🔥 Spit fire ({(direction.x > 0 ? "→" : "←")})");
    }

private Vector2 GetFacingDirection()
{
    float facing = Mathf.Sign(transform.root.localScale.x);
    return new Vector2(facing, 0f);
}

    private float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;

        return 0f;
    }

    public override bool CanDestroy(ObstacleType type) => false;
}
