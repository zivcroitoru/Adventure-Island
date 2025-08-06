using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp
{
    [System.Serializable]
    private struct WeightedPrefab
    {
        public GameObject prefab;               // Fully assigned prefab: AnimalPickup_Red, WeaponPickup, etc.
        [Range(0f, 1f)] public float weight;
    }

    [Header("Loot Table")]
    [SerializeField] private WeightedPrefab[] rewards;

    [Inject] private IObjectResolver _resolver;

    protected override void OnPickUp(GameObject player)
    {
        Debug.Log("[EggPickup] 🥚 OnPickUp triggered.");

        GameObject selectedPrefab = SelectReward();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("[EggPickup] ❌ No reward prefab selected.");
            return;
        }

        Debug.Log($"[EggPickup] 🎁 Selected reward: {selectedPrefab.name}");

        var rewardGO = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        Debug.Log($"[EggPickup] 🧸 Instantiated reward: {rewardGO.name}");

        if (_resolver == null)
        {
            Debug.LogError("[EggPickup] ❌ _resolver is null — cannot inject.");
            return;
        }

        try
        {
            _resolver.InjectGameObject(rewardGO);
            Debug.Log("[EggPickup] ✅ Dependency injection succeeded.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[EggPickup] ❌ Injection failed: {ex.Message}\n{ex.StackTrace}");
        }

        // No destroy here — base class handles it.
    }

    // ──── Local method ────
    private GameObject SelectReward()
    {
        float totalWeight = 0f;
        foreach (var r in rewards)
            totalWeight += r.weight;

        float roll = Random.value * totalWeight;
        Debug.Log($"[EggPickup] 🎲 Rolled value: {roll} / Total weight: {totalWeight}");

        foreach (var r in rewards)
        {
            if ((roll -= r.weight) <= 0f)
            {
                Debug.Log($"[EggPickup] 🧮 Chose: {r.prefab.name}");
                return r.prefab;
            }
        }

        Debug.LogWarning("[EggPickup] ❓ SelectReward fallback to null.");
        return null;
    }
}
