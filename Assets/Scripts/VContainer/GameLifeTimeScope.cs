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

    public RedAnimal RedAnimalPrefab => redAnimalPrefab;
    public BlueAnimal BlueAnimalPrefab => blueAnimalPrefab;
    public GreenAnimal GreenAnimalPrefab => greenAnimalPrefab;

    public AnimalPickup RedPickupPrefab => redPickupPrefab;
    public AnimalPickup BluePickupPrefab => bluePickupPrefab;
    public AnimalPickup GreenPickupPrefab => greenPickupPrefab;

protected override void Configure(IContainerBuilder builder)
{
    // ── Core Model ──
    builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

    // ── Pools ──
    RegisterIfExists(builder, axePool);
    RegisterIfExists(builder, firePool);
    RegisterIfExists(builder, sparkPool);
    RegisterIfExists(builder, snakeFirePool);

    // ── Weapons & Enemies in Scene ──
    builder.RegisterComponentInHierarchy<AxeWeapon>();
    builder.RegisterComponentInHierarchy<BoomerangWeapon>();
    builder.RegisterComponentInHierarchy<EnemyBase>();
    builder.RegisterComponentInHierarchy<EnemyController>();
    builder.RegisterEntryPoint<AnimalPickup>().AsSelf();


    // ── Scene Pickups ──
        builder.RegisterComponentInHierarchy<EggPickup>();

    // ✅ No need to register AnimalPickup prefabs.
    // Just make sure any [Inject] dependencies *inside them* are registered.
}


    private static void RegisterIfExists<T>(IContainerBuilder builder, T obj) where T : Component
    {
        if (obj != null)
            builder.RegisterComponent(obj);
    }
}
