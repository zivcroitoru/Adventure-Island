using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class GameLifetimeScope : LifetimeScope
{
    [Header("Projectile Pools")]
    [SerializeField] private ProjectileAxePool axePool;
    [SerializeField] private ProjectileFirePool firePool;
    [SerializeField] private ProjectileSparkPool sparkPool;
    [SerializeField] private SnakeFireProjectilePool snakeFirePool;

    [Header("Prefabs (used via resolver.Instantiate)")]
    [SerializeField] private RedAnimal redAnimalPrefab;
    [SerializeField] private BlueAnimal blueAnimalPrefab;
    [SerializeField] private GreenAnimal greenAnimalPrefab;

    [SerializeField] private AnimalPickup redPickupPrefab;
    [SerializeField] private AnimalPickup bluePickupPrefab;
    [SerializeField] private AnimalPickup greenPickupPrefab;

    // Expose prefabs to scripts via properties or accessor methods if needed
    public RedAnimal RedAnimalPrefab => redAnimalPrefab;
    public BlueAnimal BlueAnimalPrefab => blueAnimalPrefab;
    public GreenAnimal GreenAnimalPrefab => greenAnimalPrefab;

    public AnimalPickup RedPickupPrefab => redPickupPrefab;
    public AnimalPickup BluePickupPrefab => bluePickupPrefab;
    public AnimalPickup GreenPickupPrefab => greenPickupPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        // Core model
        builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

        // Projectile pools — they exist in scene, so register them
        RegisterIfExists(builder, axePool);
        RegisterIfExists(builder, firePool);
        RegisterIfExists(builder, sparkPool);
        RegisterIfExists(builder, snakeFirePool);

        // Weapons already in the scene — register from hierarchy
        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();

        // You do NOT register prefabs (animal or pickup)
        // Instead, inject them via [SerializeField] and use _resolver.Instantiate(prefab)
    }

    private static void RegisterIfExists<T>(IContainerBuilder builder, T obj) where T : Component
    {
        if (obj != null)
            builder.RegisterComponent(obj);
    }
}
