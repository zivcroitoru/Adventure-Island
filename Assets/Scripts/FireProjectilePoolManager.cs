using UnityEngine;
using System.Collections.Generic;

public class FireProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private readonly List<GameObject> pooledProjectiles = new();

    private void Awake()
    {
        Debug.Log("[FireProjectilePoolManager] Initializing pool...");

        for (int i = 0; i < amountToPool; i++)
        {
            var proj = CreateProjectile();
            Debug.Log($"[FireProjectilePoolManager] Created projectile #{i + 1}: {proj?.name}");
        }

        Debug.Log($"[FireProjectilePoolManager] Pool initialized with {pooledProjectiles.Count} projectiles.");
    }

    private GameObject CreateProjectile()
    {
        if (fireProjectilePrefab == null)
        {
            Debug.LogError("[FireProjectilePoolManager] Fire projectile prefab not assigned!");
            return null;
        }

        GameObject proj = Instantiate(fireProjectilePrefab, projectileHolder);
        proj.SetActive(false);
        pooledProjectiles.Add(proj);

        Debug.Log($"[FireProjectilePoolManager] Instantiated new projectile: {proj.name}");
        return proj;
    }

    public GameObject GetPooledProjectile()
    {
        for (int i = pooledProjectiles.Count - 1; i >= 0; i--)
        {
            GameObject proj = pooledProjectiles[i];

            if (proj == null)
            {
                Debug.LogWarning($"[FireProjectilePoolManager] Projectile at index {i} is null. Removing from pool.");
                pooledProjectiles.RemoveAt(i);
                continue;
            }

            if (!proj.activeInHierarchy)
            {
                Debug.Log($"[FireProjectilePoolManager] Reusing pooled projectile at index {i}: {proj.name}");
                return proj;
            }
        }

        Debug.LogWarning("[FireProjectilePoolManager] Pool exhausted. Creating additional projectile.");
        return CreateProjectile(); // Optional: grow pool
    }
}
