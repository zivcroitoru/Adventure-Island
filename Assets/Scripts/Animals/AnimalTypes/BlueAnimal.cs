using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class BlueAnimal : AnimalBase
{
    [Header("Spark-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;

    private ProjectileSparkPool _sparkPool;

[Inject]
public void Inject(IObjectResolver container)
{
    _sparkPool = container.Resolve<ProjectileSparkPool>();
}


    protected override void OnAttack()
    {
        if (_sparkPool == null)
        {
            Debug.LogWarning("[BlueAnimal] ❌ Spark pool not injected.");
            return;
        }

        Vector2 direction = GetFacingDirection();
        Vector2 spawnPos = CalculateSpawnPosition(direction);

        var spark = _sparkPool.Get(spawnPos, Quaternion.identity);
        if (spark == null)
        {
            Debug.LogWarning("[BlueAnimal] ❌ Failed to retrieve spark.");
            return;
        }

        float playerSpeed = GetPlayerSpeed();
        spark.Shoot(spawnPos, direction, playerSpeed);

        Debug.Log($"[BlueAnimal] ⚡ Shot spark ({(direction.x > 0 ? "→" : "←")})");
    }

    private Vector2 CalculateSpawnPosition(Vector2 direction)
    {
        Vector2 offset = new Vector2(0.8f, -0.55f);
        return (Vector2)transform.position + new Vector2(
            offset.x * Mathf.Sign(direction.x),
            offset.y
        );
    }

    private Vector2 GetFacingDirection()
    {
        if (rider == null)
            return Vector2.right;

        return rider.transform.localScale.x >= 0f ? Vector2.right : Vector2.left;
    }

    private float GetPlayerSpeed()
    {
        if (rider != null && rider.TryGetComponent(out Rigidbody2D rb))
            return rb.velocity.x;

        return 0f;
    }

    public override bool CanDestroy(ObstacleType type)
    {
        return type == ObstacleType.Rock;
    }
}
