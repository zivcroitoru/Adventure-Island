using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public sealed class FruitController : PickUp, IResettable
{
    [Header("Fruit Settings")]
    [SerializeField] private int energyBars = 1;
    [SerializeField] private Sprite fruitSprite;

    private FruitView view;
    private FruitModel model;

    private static int totalFruitsCollected;

    private bool _isCollected = false;

    public static int TotalFruitsCollected => totalFruitsCollected;

    public static event System.Action<int> OnFruitCountChanged;
    public static event System.Action OnBonusLifeEarned;

    private void Awake()
    {
        view = GetComponent<FruitView>();
        view.SetSprite(fruitSprite);

        model = new FruitModel(energyBars);

        GameResetManager.Instance?.Register(this);
    }

    protected override void OnPickUp(GameObject player)
    {
        if (_isCollected) return; // Already collected

        if (player.TryGetComponent(out AnimalBase animal) && animal.Rider != null)
        {
            player = animal.Rider;
        }

        GiveEnergy(player);
        RegisterCollection();

        _isCollected = true;
        gameObject.SetActive(false);
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

    // Reset logic
    public void ResetState()
    {
        _isCollected = false;
        gameObject.SetActive(true);
        Debug.Log("[FruitController] ResetState called: fruit reset.");

        // Optional: reset static counter somewhere central instead of here
    }

    // Static reset method for all fruits count
    public static void ResetTotalFruitsCollected()
    {
        totalFruitsCollected = 0;
        OnFruitCountChanged?.Invoke(totalFruitsCollected);
        Debug.Log("[FruitController] Total fruits collected reset.");
    }
}
