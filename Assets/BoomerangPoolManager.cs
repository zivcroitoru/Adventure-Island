using UnityEngine;
using System.Collections.Generic;

public class BoomerangPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private readonly List<GameObject> pooledBoomerangs = new();

    private void Awake()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            CreateBoomerang();
        }
    }

    private GameObject CreateBoomerang()
    {
        if (boomerangPrefab == null)
        {
            Debug.LogError("[BoomerangPoolManager] Boomerang prefab not assigned!");
            return null;
        }

        GameObject boomerang = Instantiate(boomerangPrefab, projectileHolder);
        boomerang.SetActive(false);
        pooledBoomerangs.Add(boomerang);

        return boomerang;
    }

    public GameObject GetPooledBoomerang()
    {
        for (int i = pooledBoomerangs.Count - 1; i >= 0; i--)
        {
            GameObject boomerang = pooledBoomerangs[i];

            if (boomerang == null)
            {
                pooledBoomerangs.RemoveAt(i);
                continue;
            }

            if (!boomerang.activeInHierarchy)
                return boomerang;
        }

        return CreateBoomerang(); // Optional: expand pool
    }
}
