using UnityEngine;

public class AnimalPickup : PickUp
{
    [SerializeField] private AnimalType type;
    [SerializeField] private AnimalFactory factory;

    protected override void OnPickUp(GameObject player)
    {
        var animal = factory.CreateAnimal(type);
        animal?.OnCollect(player);
    }
}
