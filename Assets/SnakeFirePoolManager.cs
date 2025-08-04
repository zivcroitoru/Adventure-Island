using UnityEngine;
using System.Collections.Generic;

public class SnakeFireProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private SnakeFireProjectile snakeFireProjectilePrefab;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private readonly List<SnakeFireProjectile> pooledProjectiles = new();

    private void Awake()
    {
        Debug.Log("[SnakeFireProjectilePoolManager] Initializing pool...");

        for (int i = 0; i < amountToPool; i++)
        {
            var proj = CreateProjectile();
            Debug.Log($"[SnakeFireProjectilePoolManager] Created projectile #{i + 1}: {proj?.name}");
        }

        Debug.Log($"[SnakeFireProjectilePoolManager] Pool initialized with {pooledProjectiles.Count} projectiles.");
    }

    private SnakeFireProjectile CreateProjectile()
    {
        if (snakeFireProjectilePrefab == null)
        {
            Debug.LogError("[SnakeFireProjectilePoolManager] Prefab not assigned!");
            return null;
        }

        var proj = Instantiate(snakeFireProjectilePrefab, projectileHolder);
        proj.gameObject.SetActive(false);
        pooledProjectiles.Add(proj);

        Debug.Log($"[SnakeFireProjectilePoolManager] Instantiated new projectile: {proj.name}");
        return proj;
    }

    public SnakeFireProjectile GetPooledProjectile()
    {
        for (int i = pooledProjectiles.Count - 1; i >= 0; i--)
        {
            var proj = pooledProjectiles[i];

            if (proj == null)
            {
                Debug.LogWarning($"[SnakeFireProjectilePoolManager] Projectile at index {i} is null. Removing.");
                pooledProjectiles.RemoveAt(i);
                continue;
            }

            if (!proj.gameObject.activeInHierarchy)
            {
                Debug.Log($"[SnakeFireProjectilePoolManager] Reusing projectile at index {i}: {proj.name}");
                return proj;
            }
        }

        Debug.LogWarning("[SnakeFireProjectilePoolManager] Pool exhausted. Expanding pool.");
        return CreateProjectile();
    }
}
