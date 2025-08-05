using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class GameLifetimeScope : LifetimeScope
{
    [Header("Projectile Pools")]
    [SerializeField] private ProjectilePool<ProjectileAxe> axePool;
    [SerializeField] private ProjectilePool<ProjectileBoomerang> boomerangPool;

    protected override void Configure(IContainerBuilder builder)
    {
        /* ───── Core model ───── */
        builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

        /* ───── Projectile Pools ───── */
        if (axePool != null)
            builder.RegisterComponent(axePool);

        if (boomerangPool != null)
            builder.RegisterComponent(boomerangPool);

        /* ───── Weapons ───── */
        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    }
}
