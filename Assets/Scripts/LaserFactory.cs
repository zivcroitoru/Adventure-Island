using UnityEngine;
using Zenject;

// Factory that builds lasers using the director and a prefab set in the Inspector
public class LaserFactory : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab; // Set in Inspector

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
