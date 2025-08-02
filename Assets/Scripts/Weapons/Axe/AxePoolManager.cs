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
        for (int i = 0; i < amountToPool; i++)
        {
            CreateAxe();
        }
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

        return axe;
    }

    public GameObject GetPooledAxe()
    {
        for (int i = pooledAxes.Count - 1; i >= 0; i--)
        {
            GameObject axe = pooledAxes[i];

            if (axe == null)
            {
                pooledAxes.RemoveAt(i); // remove destroyed reference
                continue;
            }

            if (!axe.activeInHierarchy)
                return axe;
        }

        return CreateAxe(); // Optional: grow the pool
    }
}
