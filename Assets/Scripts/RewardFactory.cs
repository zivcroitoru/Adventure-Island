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
        Spawn(prefab, position);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        GameObject reward = _resolver.Instantiate(prefab, position, Quaternion.identity);
        return reward;
    }

    private GameObject SelectRandomReward()
    {
        if (rewards.Length == 0)
        {
            return null;
        }

        // First, get the total weight of all rewards
        float totalWeight = 0f;
        foreach (var reward in rewards)
        {
            totalWeight += reward.weight;
        }

        if (totalWeight <= 0f)
        {
            return null;
        }

        // Get a random value within the range of total weight
        float randomValue = Random.Range(0f, totalWeight);

        // Loop through the rewards to find the selected one based on its weight
        float accumulatedWeight = 0f;
        foreach (var reward in rewards)
        {
            accumulatedWeight += reward.weight;
            if (randomValue <= accumulatedWeight)
            {
                Debug.Log($"Selected reward: {reward.prefab.name}");
                return reward.prefab;
            }
        }

        return null;
    }
}
