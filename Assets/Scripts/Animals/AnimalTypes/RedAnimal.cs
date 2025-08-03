using UnityEngine;

public class RedAnimal : AnimalBase
{
    [Header("Fire-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;

    private FireProjectilePoolManager firePool;

    public void InjectPool(FireProjectilePoolManager pool) => firePool = pool;

    protected override void OnAttack()
    {
        if (firePool == null)
        {
            Debug.LogWarning("[RedAnimal] Fire pool not injected.");
            return;
        }

        GameObject projGO = firePool.GetPooledProjectile();
        if (projGO == null)
        {
            Debug.LogWarning("[RedAnimal] No projectile available from pool.");
            return;
        }

        if (projGO.TryGetComponent(out ProjectileFire fire))
        {
            Vector2 direction = GetFacingDirection();
            Vector2 spawnPos = (Vector2)transform.position + direction * projectileOffset;
            fire.Shoot(spawnPos, direction);

            Debug.Log($"[RedAnimal] 🔥 Spit fire ({(direction.x > 0 ? "right" : "left")})");
        }
        else
        {
            Debug.LogWarning("[RedAnimal] Missing ProjectileFire component.");
        }
    }

    private Vector2 GetFacingDirection() => transform.lossyScale.x >= 0 ? Vector2.right : Vector2.left;

    public override bool CanDestroy(ObstacleType type) => false;
}
