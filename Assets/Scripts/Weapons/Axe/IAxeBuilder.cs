using UnityEngine;

public interface IAxeBuilder
{
    void SetSpeed();
    void SetLifetime();
    void SetSpinSpeed();
    GameObject Build(GameObject axePrefab);
}
