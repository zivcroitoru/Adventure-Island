using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public class FruitController : PickUp
{
    [Header("Fruit Settings")]
    [SerializeField] private int energyBars = 1;
    [SerializeField] private Sprite fruitSprite;

    private FruitView view;
    private static int totalFruitsCollected;

    public static event System.Action<int> OnFruitCountChanged;
    public static event System.Action OnBonusLifeEarned;

    private void Awake()
    {
        view = GetComponent<FruitView>();
        view.SetSprite(fruitSprite);
    }

    protected override void OnPickUp(GameObject player)
    {
        if (player.TryGetComponent(out AnimalBase animal) && animal.Rider != null)
        {
            player = animal.Rider;
        }

        GiveEnergy(player);
        CountFruit();
    }

    private void GiveEnergy(GameObject player)
    {
        var energy = player.GetComponentInChildren<EnergyController>();
        if (energy != null)
        {
            energy.AddBars(energyBars);
            Debug.Log($"[FruitController] Gave {energyBars} energy bars to player");
        }
        else
        {
            Debug.LogWarning("[FruitController] No EnergyController found on player");
        }
    }

    private void CountFruit()
    {
        totalFruitsCollected++;
        OnFruitCountChanged?.Invoke(totalFruitsCollected);

        if (totalFruitsCollected % 30 == 0)
        {
            Debug.Log("[FruitController] 30 fruits collected. Triggering bonus life event.");
            OnBonusLifeEarned?.Invoke();
        }
    }
}
