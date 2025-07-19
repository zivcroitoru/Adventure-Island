using UnityEngine;
using Zenject;
using System.Collections.Generic;

// Manages a pool of laser objects using the LaserFactory
public class LaserPoolManager : MonoBehaviour
{
    private List<GameObject> _pooledLasers;       // List of inactive lasers
    public Transform projectileHolder;            // Optional parent in hierarchy
    [SerializeField] private int _amountToPool = 5; // Number of lasers to pre-create

    private LaserFactory _laserFactory;           // Factory that creates lasers

    // Zenject injects the laser factory
    [Inject]
    public void Construct(LaserFactory laserFactory)
    {
        _laserFactory = laserFactory;
    }

    private void Awake()
    {
        _pooledLasers = new List<GameObject>();
    }

    private void Start()
    {
        // Preload the pool with lasers
        for (int i = 0; i < _amountToPool; i++)
            CreateLaser();
    }

    // Creates a new laser via the factory and adds it to the pool
    private GameObject CreateLaser()
    {
        ProjectileLaser laserScript = _laserFactory.CreateLaser();
        GameObject laser = laserScript.gameObject;

        laser.SetActive(false); // Don’t use it yet
        _pooledLasers.Add(laser);

        if (projectileHolder != null)
            laser.transform.SetParent(projectileHolder);

        return laser;
    }

    // Returns a laser from the pool, or creates a new one if all are used
    public GameObject GetPooledLaser()
    {
        foreach (GameObject laser in _pooledLasers)
        {
            if (!laser.activeInHierarchy)
            {
                Debug.Log("[LaserPool] Laser taken from pool.");
                return laser;
            }
        }

        Debug.Log("[LaserPool] Pool empty. Creating new laser.");
        return CreateLaser();
    }
}
