using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public sealed class FruitController : PickUp
{
    [Header("Fruit Settings")]
    [SerializeField] private int energyBars = 1;
    [SerializeField] private Sprite fruitSprite;

    private FruitView view;
    private FruitModel model;

    private static int totalFruitsCollected;

    public static int TotalFruitsCollected => totalFruitsCollected;

    public static event System.Action<int> OnFruitCountChanged;
    public static event System.Action OnBonusLifeEarned;

    private void Awake()
    {
        view = GetComponent<FruitView>();
        view.SetSprite(fruitSprite);

        model = new FruitModel(energyBars);
    }

    protected override void OnPickUp(GameObject player)
    {
        if (player.TryGetComponent(out AnimalBase animal) && animal.Rider != null)
        {
            player = animal.Rider;
        }

        GiveEnergy(player);
        RegisterCollection();
    }

    private void GiveEnergy(GameObject player)
    {
        var energy = player.GetComponentInChildren<EnergyController>();
        if (energy != null)
        {
            energy.AddBars(model.EnergyValue);
            Debug.Log($"[FruitController] Gave {model.EnergyValue} energy bars to player");
        }
        else
        {
            Debug.LogWarning("[FruitController] No EnergyController found on player");
        }
    }

    private void RegisterCollection()
    {
        totalFruitsCollected++;
        BroadcastFruitCount();

        if (totalFruitsCollected % 30 == 0)
        {
            Debug.Log("[FruitController] 🎉 30 fruits collected. Bonus life awarded!");
            OnBonusLifeEarned?.Invoke();
        }
    }

    private void BroadcastFruitCount()
    {
        OnFruitCountChanged?.Invoke(totalFruitsCollected);
    }
}
