using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Axe System")]
    [SerializeField] private AxePoolManager axePoolManager;

    protected override void Configure(IContainerBuilder builder)
    {
        var energyModel = new EnergyModel(45f);
        builder.RegisterInstance<IEnergyModel>(energyModel);

        // Axe System
        if (axePoolManager != null)
        {
            builder.RegisterComponent(axePoolManager);
        }
        builder.RegisterComponentInHierarchy<AxeWeapon>();

        // BoomerangWeapon no longer uses a pool — just grab from hierarchy
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    }
}
