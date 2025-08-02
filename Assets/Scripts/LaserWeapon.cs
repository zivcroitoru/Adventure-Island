using UnityEngine;
using VContainer;

public class LaserWeapon : MonoBehaviour, IUseableWeapon
{
    public GameObject laser;
    private bool _isEquip = false;
    private LaserPoolManager _laserPoolManager;

    public void Construct(LaserPoolManager laserPoolManager)
    {
        _laserPoolManager = laserPoolManager;
    }

    public void Shoot()
    {
        if (laser != null && _isEquip)
        {
            GameObject curLaser = _laserPoolManager.GetPooledLaser();
            Debug.Log("[LaserPool] Laser taken from pool.");

            curLaser.transform.position = transform.position;
            curLaser.SetActive(true);

            ProjectileLaser scLaser = curLaser.GetComponent<ProjectileLaser>();
            if (scLaser != null)
            {
                scLaser.Shoot();
                Debug.Log("[LaserWeapon] Laser fired.");
            }
        }
        else if (!_isEquip)
        {
            Debug.Log("[LaserWeapon] Cannot shoot – not equipped.");
        }
    }

    public void Equip() => _isEquip = true;
    public void UnEquip() => _isEquip = false;
}
