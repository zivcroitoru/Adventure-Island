using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Axe System")]
    [SerializeField] private AxePoolManager axePoolManager;

    [Header("Fire System")]
    [SerializeField] private FireProjectilePoolManager fireProjectilePoolManager;

    [Header("Spark System")]
    [SerializeField] private SparkProjectilePoolManager sparkProjectilePoolManager;

    protected override void Configure(IContainerBuilder builder)
    {
        // Energy system
        var energyModel = new EnergyModel(45f);
        builder.RegisterInstance<IEnergyModel>(energyModel);

        // Axe System
        if (axePoolManager != null)
            builder.RegisterComponent(axePoolManager);
        builder.RegisterComponentInHierarchy<AxeWeapon>();

        // Fire System
        if (fireProjectilePoolManager != null)
            builder.RegisterComponent(fireProjectilePoolManager);

        // Spark System
        if (sparkProjectilePoolManager != null)
            builder.RegisterComponent(sparkProjectilePoolManager);

        // Boomerang Weapon (non-pooled)
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    }
}
