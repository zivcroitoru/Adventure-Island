using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{
    [Header("Projectile Pools")]
    [SerializeField] private AxeProjectilePool _axePool;
    // [SerializeField] private BoomerangProjectilePool _boomerangPool;

    /// <summary>
    /// Spawns an axe projectile from the pool.
    /// </summary>
    public ProjectileAxe SpawnAxe(Vector3 position, Quaternion rotation)
        => _axePool.Spawn(position, rotation);

    /// <summary>
    /// Provides access to the internal ObjectPool for advanced control.
    /// </summary>
    public ObjectPool<ProjectileAxe> AxePool => _axePool.Pool;

    // Future example:
    // public ProjectileBoomerang SpawnBoomerang(Vector3 pos, Quaternion rot)
    //     => _boomerangPool.Spawn(pos, rot);

    // public ObjectPool<ProjectileBoomerang> BoomerangPool => _boomerangPool.Pool;
}
