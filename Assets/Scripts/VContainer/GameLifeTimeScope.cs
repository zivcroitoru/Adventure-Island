using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class GameLifetimeScope : LifetimeScope
{
    [Header("Managers")]
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private AxeProjectilePool axeProjectilePool;

    protected override void Configure(IContainerBuilder builder)
    {
        /* ───── Core model ───── */
        builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

        /* ───── Managers ───── */
        if (poolManager != null)
            builder.RegisterComponent(poolManager);

        if (axeProjectilePool != null)
            builder.RegisterComponent(axeProjectilePool);

        /* ───── Weapons ───── */
        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    }
}
