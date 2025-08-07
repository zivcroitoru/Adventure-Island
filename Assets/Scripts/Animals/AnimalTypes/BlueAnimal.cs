using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class BlueAnimal : AnimalBase
{
    private ProjectileSparkPool _sparkPool;

    [Inject]
    public void Inject(IObjectResolver container)
    {
        _sparkPool = container.Resolve<ProjectileSparkPool>();
    }

    protected override void OnAttack()
    {
        Vector2 direction = GetFacingDirection();
        Vector2 spawnPos = CalculateSpawnPosition(direction);

        var spark = _sparkPool.Get(spawnPos, Quaternion.identity);
        if (spark == null) return;

        spark.Shoot(spawnPos, direction, GetPlayerSpeed());
    }

    private Vector2 CalculateSpawnPosition(Vector2 direction)
    {
        Vector2 offset = new Vector2(0.8f, -0.55f);
        return (Vector2)transform.position + new Vector2(
            offset.x * Mathf.Sign(direction.x),
            offset.y
        );
    }
}
