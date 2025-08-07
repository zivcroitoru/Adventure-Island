using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// Root container for the level: registers pools, weapons, enemies, and factories.
/// </summary>
public sealed class GameLifetimeScope : LifetimeScope
{
    /* ───── Serialized dependencies ───── */
    [Header("Projectile Pools")]
    [SerializeField] ProjectileAxePool        axePool;
    [SerializeField] ProjectileFirePool       firePool;
    [SerializeField] ProjectileSparkPool      sparkPool;
    [SerializeField] SnakeFireProjectilePool  snakeFirePool;
    [SerializeField] ProjectileBoomerangPool  boomerangPool;

    /* ── Prefabs accessed via resolver.Instantiate (kept public for designer) ── */
    [Header("Animal Prefabs")]
    [SerializeField] RedAnimal    redAnimalPrefab;
    [SerializeField] BlueAnimal   blueAnimalPrefab;
    [SerializeField] GreenAnimal  greenAnimalPrefab;

    [Header("Pickup Prefabs")]
    [SerializeField] AnimalPickup redPickupPrefab;
    [SerializeField] AnimalPickup bluePickupPrefab;
    [SerializeField] AnimalPickup greenPickupPrefab;

    /* ───── Read-only accessors (optional, for other scripts) ───── */
    public RedAnimal    RedAnimalPrefab   => redAnimalPrefab;
    public BlueAnimal   BlueAnimalPrefab  => blueAnimalPrefab;
    public GreenAnimal  GreenAnimalPrefab => greenAnimalPrefab;

    public AnimalPickup RedPickupPrefab   => redPickupPrefab;
    public AnimalPickup BluePickupPrefab  => bluePickupPrefab;
    public AnimalPickup GreenPickupPrefab => greenPickupPrefab;

    /* ───── Container config ───── */
    protected override void Configure(IContainerBuilder builder)
    {
        /* Core model */
        builder.RegisterInstance<IEnergyModel>(new EnergyModel(45f));

        /* Pools (only if present on this scope) */
        RegisterIfExists(builder, axePool);
        RegisterIfExists(builder, firePool);
        RegisterIfExists(builder, sparkPool);
        RegisterIfExists(builder, snakeFirePool);
        RegisterIfExists(builder, boomerangPool);

        /* Gameplay logic */
        builder.Register<SnakeFireAttackStrategy>(Lifetime.Scoped);

        /* Scene objects */
        builder.RegisterComponentInHierarchy<AxeWeapon>();
        builder.RegisterComponentInHierarchy<BoomerangWeapon>();
        builder.RegisterComponentInHierarchy<EnemyController>();
        builder.RegisterComponentInHierarchy<RewardFactory>();   // Reward spawner (now self-injecting)
        builder.RegisterComponentInHierarchy<SnakeFireAttackStrategy>();


        /* Optional: player’s extra components if found at runtime */
        RegisterIfExists(builder, GameObject.FindWithTag("Player")?.GetComponent<AxeWeapon>());
    }

    private static void RegisterIfExists<T>(IContainerBuilder builder, T component)
        where T : Component
    {
        if (component != null)
            builder.RegisterComponent(component);
    }
}
