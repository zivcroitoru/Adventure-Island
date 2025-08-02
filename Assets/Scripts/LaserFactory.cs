using UnityEngine;
using VContainer;

public class LaserFactory : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab; // Assigned in Inspector

    private LaserDirector _laserDirector;

    [Inject]
    public void Construct(LaserDirector director)
    {
        _laserDirector = director;
    }

    public ProjectileLaser CreateLaser()
    {
        GameObject laser = _laserDirector.Construct(_laserPrefab);
        return laser.GetComponent<ProjectileLaser>();
    }
}
