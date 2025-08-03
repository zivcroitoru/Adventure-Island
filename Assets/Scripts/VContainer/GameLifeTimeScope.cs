using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Axe System")]
    [SerializeField] private AxePoolManager axePoolManager;

    [Header("Fire System")]
    [SerializeField] private FireProjectilePoolManager fireProjectilePoolManager;

    protected override void Configure(IContainerBuilder builder)
    {
        // Energy system
        var energyModel = new EnergyModel(45f);
        builder.RegisterInstance<IEnergyModel>(energyModel);

        // Axe System
        if (axePoolManager != null)
        {
            builder.RegisterComponent(axePoolManager);
        }
        builder.RegisterComponentInHierarchy<AxeWeapon>();

        // Fire System
        if (fireProjectilePoolManager != null)
        {
            builder.RegisterComponent(fireProjectilePoolManager);
        }

        // Boomerang Weapon (non-pooled)
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();

        // Register RedAnimal if you want to resolve it via VContainer
        // builder.Register<RedAnimal>(Lifetime.Transient); // Only if VContainer instantiates it

        // If you're using an AnimalFactory or RedAnimal is spawned manually,
        // you don’t need to register it here. Instead, inject pool manually post-instantiate.
    }
}
