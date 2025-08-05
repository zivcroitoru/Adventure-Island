using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp
{
    [System.Serializable]
    private struct WeightedPrefab
    {
        public GameObject prefab;               // AnimalPickup, WeaponPickup, Fairy, etc.
        [Range(0f, 1f)] public float weight;
        public AnimalBase animalPrefab;         // Optional: only used if prefab is AnimalPickup
    }

    [Header("Loot Table")]
    [SerializeField] private WeightedPrefab[] rewards;

    [Inject] private IObjectResolver _resolver;

    protected override void OnPickUp(GameObject player)
    {
        GameObject selectedPrefab = SelectReward();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("[EggPickup] ❌ No reward prefab selected.");
            return;
        }

        var rewardGO = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        

        // Inject dependencies into spawned object
        _resolver.InjectGameObject(rewardGO);

        // Handle AnimalPickup specifically (assign animal prefab)
        if (rewardGO.TryGetComponent<AnimalPickup>(out var animalPickup))
        {
            var rewardData = GetSelectedRewardData(selectedPrefab);
            if (rewardData.animalPrefab != null)
                animalPickup.SetAnimal(rewardData.animalPrefab);
        }

        // Destroy the egg
        Destroy(gameObject);
    }

    GameObject SelectReward()
    {
        float totalWeight = 0f;
        foreach (var r in rewards)
            totalWeight += r.weight;

        float roll = Random.value * totalWeight;
        foreach (var r in rewards)
        {
            if ((roll -= r.weight) <= 0f)
                return r.prefab;
        }
        return null;
    }

    WeightedPrefab GetSelectedRewardData(GameObject selected)
    {
        foreach (var r in rewards)
            if (r.prefab == selected)
                return r;
        return default;
    }
}
