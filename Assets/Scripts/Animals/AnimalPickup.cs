using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class AnimalPickup : PickUp
{
    [Header("Animal Pickup Config")]
    [SerializeField] private AnimalBase animalPrefab;

    private IObjectResolver _resolver;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public void SetAnimal(AnimalBase prefab)
    {
        animalPrefab = prefab;
    }

    protected override void OnPickUp(GameObject player)
    {
        if (animalPrefab == null)
        {
            Debug.LogError($"[AnimalPickup] ❌ animalPrefab not assigned on '{gameObject.name}'");
            return;
        }

        if (_resolver == null)
        {
            Debug.LogError($"[AnimalPickup] ❌ IObjectResolver not injected into '{gameObject.name}'");
            return;
        }

        var animal = _resolver.Instantiate(animalPrefab, transform.position, Quaternion.identity);
        animal.OnCollect(player);
    }
}
