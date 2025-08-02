using UnityEngine;
using VContainer;
using System.Collections.Generic;

public class LaserPoolManager : MonoBehaviour
{
    private List<GameObject> _pooledLasers;
    [SerializeField] private int _amountToPool = 5;
    [SerializeField] private Transform projectileHolder;

    private LaserFactory _laserFactory;

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
        for (int i = 0; i < _amountToPool; i++)
            CreateLaser();
    }

    private GameObject CreateLaser()
    {
        ProjectileLaser laserScript = _laserFactory.CreateLaser();
        GameObject laser = laserScript.gameObject;

        laser.SetActive(false);
        _pooledLasers.Add(laser);

        if (projectileHolder != null)
            laser.transform.SetParent(projectileHolder);

        return laser;
    }

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
