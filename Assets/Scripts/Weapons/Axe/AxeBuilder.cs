using UnityEngine;

public class AxeBuilder : IAxeBuilder
{
    private float xSpeed;
    private float ySpeed;
    private float destroyTime;
    private float spinSpeed;

    private readonly AxePoolManager _poolManager;

    public AxeBuilder(AxePoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public void SetSpeed()
    {
        xSpeed = 5f;
        ySpeed = 5f;
    }

    public void SetLifetime()
    {
        destroyTime = 5f;
    }

    public void SetSpinSpeed()
    {
        spinSpeed = 720f;
    }

    public GameObject Build(GameObject axePrefab)
    {
        GameObject newAxe = _poolManager.GetPooledAxe();
        if (newAxe == null)
        {
            Debug.LogError("[AxeBuilder] AxePoolManager returned null. Axe not built.");
            return null;
        }

        if (newAxe.TryGetComponent(out ProjectileAxe axe))
        {
            axe.xSpeed = xSpeed;
            axe.ySpeed = ySpeed;
            axe.destroyTime = destroyTime;
            axe.spinSpeed = spinSpeed;
        }
        else
        {
            Debug.LogWarning("[AxeBuilder] Pooled object is missing ProjectileAxe component.");
        }

        return newAxe;
    }
}
