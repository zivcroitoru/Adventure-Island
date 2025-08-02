using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Axe System")]
    [SerializeField] private AxePoolManager axePoolManager;
    [SerializeField] private AxeFactory axeFactory;

    protected override void Configure(IContainerBuilder builder)
    {
        var energyModel = new EnergyModel(45f);
        builder.RegisterInstance<IEnergyModel>(energyModel);

        if (axeFactory != null)
        {
            builder.RegisterComponent(axeFactory);
        }

        if (axePoolManager != null)
        {
            builder.RegisterComponent(axePoolManager);
        }

        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.Register<IAxeBuilder, AxeBuilder>(Lifetime.Singleton);
        builder.Register<AxeDirector>(Lifetime.Singleton);
    }
}
