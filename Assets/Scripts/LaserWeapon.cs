using UnityEngine;
using Zenject;

// Controls shooting logic for the laser weapon
public class LaserWeapon : MonoBehaviour, IUseableWeapon
{
    public GameObject laser; 
    private bool _isEquip = false;

    [Inject] private LaserPoolManager _laserPoolManager;

    public void Shoot()
    {
        if (laser != null && _isEquip)
        {
            // Take laser from pool
            GameObject curLaser = _laserPoolManager.GetPooledLaser();
            Debug.Log("[LaserPool] Laser taken from pool.");

            // Set its position and activate
            curLaser.transform.position = transform.position;
            curLaser.SetActive(true);

            // Tell it to shoot
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
