using UnityEngine;

public class AnimalPickup : PickUp
{
    [SerializeField] private AnimalType type;
    [SerializeField] private AnimalFactory factory;

    protected override void OnPickUp(GameObject player)
    {
        Debug.Log($"[AnimalPickup] Player '{player.name}' picked up animal type: {type}");

        var animal = factory.CreateAnimal(type);
        if (animal == null)
        {
            Debug.LogError("[AnimalPickup] Failed to create animal from factory!");
            return;
        }

        // 🔁 Inject Fire Pool if applicable
        if (animal is RedAnimal redAnimal)
        {
            var firePool = FindObjectOfType<FireProjectilePoolManager>();
            redAnimal.InjectPool(firePool);
        }

        Debug.Log($"[AnimalPickup] Created animal: {animal.name}");
        animal.OnCollect(player);
    }
}
