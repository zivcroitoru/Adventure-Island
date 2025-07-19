using UnityEngine;

public interface ILaserBuilder
{
    void SetSpeed();
    void SetLifeTime();
    GameObject Build(GameObject laserPrefab);
}
