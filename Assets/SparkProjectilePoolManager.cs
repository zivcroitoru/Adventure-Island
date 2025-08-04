using UnityEngine;
using System.Collections.Generic;

public class SparkProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject sparkProjectilePrefab;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private readonly List<GameObject> pooledProjectiles = new();

    private void Awake()
    {
        Debug.Log("[SparkProjectilePoolManager] Initializing pool...");

        for (int i = 0; i < amountToPool; i++)
        {
            var proj = CreateProjectile();
            Debug.Log($"[SparkProjectilePoolManager] Created spark #{i + 1}: {proj?.name}");
        }

        Debug.Log($"[SparkProjectilePoolManager] Pool initialized with {pooledProjectiles.Count} sparks.");
    }

    private GameObject CreateProjectile()
    {
        if (sparkProjectilePrefab == null)
        {
            Debug.LogError("[SparkProjectilePoolManager] Spark projectile prefab not assigned!");
            return null;
        }

        GameObject proj = Instantiate(sparkProjectilePrefab, projectileHolder);
        proj.SetActive(false);
        pooledProjectiles.Add(proj);

        Debug.Log($"[SparkProjectilePoolManager] Instantiated new spark: {proj.name}");
        return proj;
    }

    public GameObject GetPooledProjectile()
    {
        for (int i = pooledProjectiles.Count - 1; i >= 0; i--)
        {
            GameObject proj = pooledProjectiles[i];

            if (proj == null)
            {
                Debug.LogWarning($"[SparkProjectilePoolManager] Spark at index {i} is null. Removing from pool.");
                pooledProjectiles.RemoveAt(i);
                continue;
            }

            if (!proj.activeInHierarchy)
            {
                Debug.Log($"[SparkProjectilePoolManager] Reusing pooled spark at index {i}: {proj.name}");
                return proj;
            }
        }

        Debug.LogWarning("[SparkProjectilePoolManager] Pool exhausted. Creating additional spark.");
        return CreateProjectile(); // Optional: grow pool
    }
}
