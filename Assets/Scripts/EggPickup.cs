using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp
{
    [System.Serializable]
    private struct WeightedPrefab
    {
        public GameObject prefab; // Fully assigned prefab: AnimalPickup_Red, WeaponPickup, etc.
        [Range(0f, 1f)] public float weight;
    }

    [Header("Loot Table")]
    [SerializeField] private WeightedPrefab[] rewards;

    private IObjectResolver _resolver;

    public void SetResolver(IObjectResolver resolver)
    {
        _resolver = resolver;
    }
private void Start()
{
    if (_resolver == null)
    {
        var scope = FindFirstObjectByType<LifetimeScope>();
        if (scope != null)
        {
            _resolver = scope.Container;
            Debug.Log($"[EggPickup] 🧩 Auto-assigned resolver on scene egg: {name}");
        }
        else
        {
            Debug.LogWarning("[EggPickup] ⚠️ No LifetimeScope found in scene.");
        }
    }
}





    protected override void OnPickUp(GameObject player)
    {
        Debug.Log("[EggPickup] 🥚 OnPickUp triggered.");

        var selectedPrefab = SelectReward();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("[EggPickup] ❌ No reward prefab selected.");
            return;
        }

        Debug.Log($"[EggPickup] 🎁 Selected reward: {selectedPrefab.name}");

        var rewardGO = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        InjectReward(rewardGO);
    }

    private void InjectReward(GameObject rewardGO)
    {
        if (_resolver == null)
        {
            Debug.LogError("[EggPickup] ❌ _resolver is null — cannot inject.");
            return;
        }

        var components = rewardGO.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var comp in components)
            Debug.Log($"[EggPickup] 🔍 Component on reward: {comp.GetType().Name}");

        try
        {
            _resolver.InjectGameObject(rewardGO);
            Debug.Log("[EggPickup] ✅ Dependency injection succeeded.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[EggPickup] ❌ Injection crash:\n{ex.Message}\n{ex.StackTrace}");
        }
    }

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
