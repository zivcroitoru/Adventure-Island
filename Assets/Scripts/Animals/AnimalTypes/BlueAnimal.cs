using UnityEngine;

public class BlueAnimal : AnimalBase
{
    [Header("Spark-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;

    private SparkProjectilePoolManager sparkPool;

    public void InjectPool(SparkProjectilePoolManager pool) => sparkPool = pool;

protected override void OnAttack()
{
    if (sparkPool == null)
    {
        Debug.LogWarning("[BlueAnimal] ❌ Spark pool not injected.");
        return;
    }

    var projGO = sparkPool.GetPooledProjectile();
    if (projGO == null)
    {
        Debug.LogWarning("[BlueAnimal] ❌ No spark available from pool.");
        return;
    }

    if (!projGO.TryGetComponent(out ProjectileSpark spark))
    {
        Debug.LogWarning("[BlueAnimal] ❌ Pooled object missing ProjectileSpark.");
        return;
    }

    var direction = GetFacingDirection();
    Vector2 offset = new Vector2(0.8f, -0.55f); // ⬅️ adjusted
    Vector2 spawnPos = (Vector2)transform.position + new Vector2(
        offset.x * Mathf.Sign(direction.x),
        offset.y
    );

    Debug.DrawLine(transform.position, spawnPos, Color.blue, 1f); // 👀 now blue = correct

    var playerSpeed = GetPlayerSpeed();
    spark.Shoot(spawnPos, direction, playerSpeed);

    Debug.Log($"[BlueAnimal] ⚡ Shot spark ({(direction.x > 0 ? "→" : "←")})");
}


private Vector2 GetFacingDirection()
{
    if (rider == null) return Vector2.right;

    float riderScale = rider.transform.localScale.x;
    return riderScale >= 0f ? Vector2.right : Vector2.left;
}



    private float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;

        return 0f;
    }

    public override bool CanDestroy(ObstacleType type) => type == ObstacleType.Rock;
}
