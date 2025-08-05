using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private ProjectileAxePool axePool;
    [SerializeField] private ProjectileFirePool firePool;
    [SerializeField] private ProjectileSparkPool sparkPool;
    [SerializeField] private SnakeFireProjectilePool snakeFirePool;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

        if (axePool != null) builder.RegisterComponent(axePool);
        if (firePool != null) builder.RegisterComponent(firePool);
        if (sparkPool != null) builder.RegisterComponent(sparkPool);
        if (snakeFirePool != null) builder.RegisterComponent(snakeFirePool);

        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    }
}
