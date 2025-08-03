using UnityEngine;
using System.Collections.Generic;

public class AxePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private readonly List<GameObject> pooledAxes = new();

    private void Awake()
    {
        Debug.Log("[AxePoolManager] Initializing pool...");

        for (int i = 0; i < amountToPool; i++)
        {
            var axe = CreateAxe();
            Debug.Log($"[AxePoolManager] Created axe #{i + 1}: {axe?.name}");
        }

        Debug.Log($"[AxePoolManager] Pool initialized with {pooledAxes.Count} projectiles.");
    }

    private GameObject CreateAxe()
    {
        if (axePrefab == null)
        {
            Debug.LogError("[AxePoolManager] Axe prefab not assigned!");
            return null;
        }

        GameObject axe = Instantiate(axePrefab, projectileHolder);
        axe.SetActive(false);
        pooledAxes.Add(axe);

        Debug.Log($"[AxePoolManager] Instantiated new axe: {axe.name}");
        return axe;
    }

    public GameObject GetPooledAxe()
    {
        for (int i = pooledAxes.Count - 1; i >= 0; i--)
        {
            GameObject axe = pooledAxes[i];

            if (axe == null)
            {
                Debug.LogWarning($"[AxePoolManager] Axe at index {i} is null. Removing from pool.");
                pooledAxes.RemoveAt(i);
                continue;
            }

            if (!axe.activeInHierarchy)
            {
                Debug.Log($"[AxePoolManager] Reusing pooled axe at index {i}: {axe.name}");
                return axe;
            }
        }

        Debug.LogWarning("[AxePoolManager] Pool exhausted. Creating additional axe.");
        return CreateAxe(); // Optional: grow pool
    }
}
