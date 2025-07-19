using UnityEngine;

// Builds a laser with custom speed and lifetime
public class LaserBuilder : ILaserBuilder
{
    private float speed;     // How fast the laser moves
    private float lifeTime;  // How long the laser lasts

    // Set how fast the laser should go
    public void SetSpeed()
    {
        this.speed = 20;
    }

    // Set how long the laser should stay before disappearing
    public void SetLifeTime()
    {
        this.lifeTime = 1.5f;
    }

    // Create the laser and apply the speed and lifetime settings
    public GameObject Build(GameObject laserPrefab)
    {
        // Make a copy of the laser prefab
        GameObject newLaser = Object.Instantiate(laserPrefab);

        // Set up the laser's speed and lifetime
        ProjectileLaser laserComponent = newLaser.GetComponent<ProjectileLaser>();
        laserComponent.Initialized(speed, lifeTime);

        return newLaser;
    }
}
