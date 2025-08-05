using UnityEngine;

public class BlueAnimal : AnimalBase
{
    [Header("Spark-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;
    [SerializeField] private ProjectileSparkPool sparkPool; // ✅ Use new pool

    protected override void OnAttack()
    {
        if (sparkPool == null)
        {
            Debug.LogWarning("[BlueAnimal] ❌ Spark pool not assigned.");
            return;
        }

        var direction = GetFacingDirection();
        Vector2 offset = new Vector2(0.8f, -0.55f);
        Vector2 spawnPos = (Vector2)transform.position + new Vector2(
            offset.x * Mathf.Sign(direction.x),
            offset.y
        );

        var spark = sparkPool.Get(spawnPos, Quaternion.identity); // ✅ Pooled spark
        if (spark == null)
        {
            Debug.LogWarning("[BlueAnimal] ❌ Failed to retrieve spark.");
            return;
        }

        var playerSpeed = GetPlayerSpeed();
        spark.Shoot(spawnPos, direction, playerSpeed);

        Debug.Log($"[BlueAnimal] ⚡ Shot spark ({(direction.x > 0 ? "→" : "←")})");
    }

    private Vector2 GetFacingDirection()
    {
        if (rider == null) return Vector2.right;
        return rider.transform.localScale.x >= 0f ? Vector2.right : Vector2.left;
    }

    private float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;

        return 0f;
    }

    public override bool CanDestroy(ObstacleType type) => type == ObstacleType.Rock;
}
