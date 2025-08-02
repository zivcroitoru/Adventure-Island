using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Controllers")]
    [SerializeField] private PlayerDamageController damageController;

    [Header("Factories")]
    [SerializeField] private LaserFactory laserFactory;


    [Header("Pool Managers")]
    [SerializeField] private LaserPoolManager laserPoolManager;
    [SerializeField] private FireballPoolManager fireballPoolManager;

    protected override void Configure(IContainerBuilder builder)
    {
        // Register energy model (singleton logic)
        var energyModel = new EnergyModel(45f);
        builder.RegisterInstance<IEnergyModel>(energyModel);

        // Register MonoBehaviour components (must be assigned in the Inspector)
        builder.RegisterComponent(damageController);
        builder.RegisterComponent(laserFactory);
        builder.RegisterComponent(laserPoolManager);
        builder.RegisterComponent(fireballPoolManager);
        builder.Register<IFireballBuilder, FireballBuilder>(Lifetime.Singleton);


        // Register builders & directors
        builder.Register<ILaserBuilder, LaserBuilder>(Lifetime.Singleton);
        builder.Register<LaserDirector>(Lifetime.Singleton);
        builder.Register<FireballDirector>(Lifetime.Singleton);
    }
}
