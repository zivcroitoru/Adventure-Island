using UnityEngine;

public class FruitEffect : MonoBehaviour, IResettable
{
    [SerializeField] private int energyBars = 1;

    private static int totalFruitsCollected;

    // ✅ Public getter so UI can access the fruit count
    public static int TotalFruitsCollected => totalFruitsCollected;

    public static event System.Action<int> OnFruitCountChanged;
    public static event System.Action OnBonusLifeEarned;

    public void Apply(GameObject player)
    {
        if (player.GetComponentInChildren<EnergyController>() is EnergyController energy)
        {
            energy.AddBars(energyBars);
            Debug.Log($"[FruitEffect] Gave {energyBars} energy.");
        }

        totalFruitsCollected++;
        OnFruitCountChanged?.Invoke(totalFruitsCollected);

        if (totalFruitsCollected % 30 == 0)
        {
            OnBonusLifeEarned?.Invoke();
        }
    }
    private void Awake() =>
        GameResetManager.Instance?.Register(this);   // join the reset list

    public void ResetState() => ResetFruitCount();   // zero the counter
    public static void ResetFruitCount()
    {
        totalFruitsCollected = 0;
        OnFruitCountChanged?.Invoke(totalFruitsCollected);
        Debug.Log("[FruitEffect] Total fruits reset.");
    }
}
