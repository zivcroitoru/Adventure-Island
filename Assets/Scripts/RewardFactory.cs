using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class RewardFactory : MonoBehaviour
{
    public static RewardFactory Instance { get; private set; }

    [System.Serializable]
    public struct WeightedPrefab
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float weight;
    }

    [Header("Global Reward Table")]
    [SerializeField] private WeightedPrefab[] rewards;

    private IObjectResolver _resolver;   // injected by VContainer

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Inject]
    public void Construct(IObjectResolver resolver) => _resolver = resolver;

    public void SpawnRandomReward(Vector3 position)
    {
        GameObject prefab = SelectRandomReward();
        if (prefab == null)
        {
            Debug.Log("No reward selected. Check if the reward table is populated and weights are set.");
            return; // If no prefab is selected, exit
        }

        Debug.Log($"Spawning reward at position {position}");
        Spawn(prefab, position);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        GameObject reward = _resolver.Instantiate(prefab, position, Quaternion.identity);
        if (reward != null)
        {
            Debug.Log($"Reward spawned: {reward.name}");
        }
        else
        {
            Debug.LogError("Failed to instantiate reward prefab.");
        }
        return reward;
    }

    private GameObject SelectRandomReward()
    {
        float total = 0f;
        foreach (var r in rewards)
        {
            total += r.weight;
            Debug.Log($"Reward: {r.prefab.name}, Weight: {r.weight}");
        }

        if (total <= 0f)
        {
            Debug.LogWarning("Total weight is 0 or negative. Cannot select a reward.");
            return null;
        }

        float roll = Random.value * total;
        Debug.Log($"Roll value: {roll} / Total weight: {total}");

        foreach (var r in rewards)
        {
            roll -= r.weight;
            if (roll <= 0f)
            {
                Debug.Log($"Selected reward: {r.prefab.name}");
                return r.prefab;
            }
        }

        Debug.LogWarning("No reward selected, check weights and table.");
        return null;
    }

    // Method to reset the reward state (you can call this from other parts of your game when the reward is collected or reset)
    public void ResetRewardState()
    {
        Debug.Log("Reward state reset.");
    }
}
