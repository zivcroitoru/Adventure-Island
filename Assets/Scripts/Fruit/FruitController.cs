using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public class FruitController : PickUp
{
    [Header("Fruit Settings")]
    [SerializeField] private int energyBars = 1;
    [SerializeField] private Sprite fruitSprite;

    private FruitView view;

    private void Awake()
    {
        view = GetComponent<FruitView>();
        view.SetSprite(fruitSprite);
    }
protected override void OnPickUp(GameObject player)
{
    // ✅ We're riding, so resolve actual player from the animal
    if (player.TryGetComponent(out AnimalBase animal))
    {
        if (animal.Rider != null)
        {
            player = animal.Rider;
        }
    }

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


}
