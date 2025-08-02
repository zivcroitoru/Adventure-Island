using UnityEngine;

public class AxeDirector
{
    private IAxeBuilder _builder;

    public AxeDirector(IAxeBuilder builder)
    {
        _builder = builder;
    }

    public GameObject Construct(GameObject axePrefab)
    {
        _builder.SetSpeed();
        _builder.SetLifetime();
        _builder.SetSpinSpeed();
        return _builder.Build(axePrefab);
    }
}
