using UnityEngine;

public class RedAnimal : AnimalBase
{
    [Header("Fire-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;
    [SerializeField] private ProjectileFirePool firePool; // ✅ Replace custom manager

    protected override void OnAttack()
    {
        if (firePool == null)
        {
            Debug.LogWarning("[RedAnimal] ❌ Fire pool not assigned.");
            return;
        }

        Vector2 direction = GetFacingDirection();
        Vector2 spawnPos = (Vector2)transform.position + direction * projectileOffset;

        var fire = firePool.Get(spawnPos, Quaternion.identity); // ✅ Pooled fireball
        if (fire == null)
        {
            Debug.LogWarning("[RedAnimal] ❌ Failed to get fire projectile.");
            return;
        }

        float playerSpeed = GetPlayerSpeed();
        fire.Shoot(spawnPos, direction, playerSpeed);

        Debug.Log($"[RedAnimal] 🔥 Spit fire ({(direction.x > 0 ? "→" : "←")})");
    }

    private Vector2 GetFacingDirection() => transform.lossyScale.x >= 0 ? Vector2.right : Vector2.left;

    private float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;

        return 0f;
    }

    public override bool CanDestroy(ObstacleType type) => false;
}
