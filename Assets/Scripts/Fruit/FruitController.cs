using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public class FruitController : PickUp
{
    [Header("Fruit Settings")]
    [SerializeField] private int energyBars = 1;  // Define how many bars this fruit gives
    [SerializeField] private Sprite fruitSprite;

    private FruitView view;

    private void Awake()
    {
        view = GetComponent<FruitView>();
        view.SetSprite(fruitSprite);

        Debug.Log($"[FruitController] Initialized with energy bars: {energyBars} and sprite: {fruitSprite?.name}");
    }

    protected override void OnPickUp(GameObject player)
    {
        Debug.Log($"[FruitController] Picked up by: {player.name}");

        var energy = player.GetComponentInChildren<EnergyController>();
        if (energy != null)
        {
            Debug.Log($"[FruitController] Adding {energyBars} bar(s) to player");
            energy.AddBars(energyBars);  // ✅ This is now bar-based
        }
        else
        {
            Debug.LogWarning("[FruitController] EnergyController not found on player or its children");
        }
    }
}
