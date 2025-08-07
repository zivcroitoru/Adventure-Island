using UnityEngine;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp, IResettable
{
    [System.Serializable]
    private struct WeightedPrefab
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float weight;
    }

    [Header("Loot Table")]
    [SerializeField] private WeightedPrefab[] rewards;

    [Header("Pickup Settings")]
    [SerializeField] private float pickupDelay = 0.3f;
    [SerializeField] private Collider2D pickupCollider;

    private RewardFactory rewardFactory;   // set by EggDropper
    private bool collected;

    /* ------------- Public API ------------- */
    public void SetRewardFactory(RewardFactory factory)
    {
        rewardFactory = factory;
        Debug.Log($"[EggPickup] 🏭 RewardFactory set: {(rewardFactory == null ? "❌ NULL" : "✅ OK")}", this);
    }
private void Start()
{
    if (rewardFactory == null)
    {
        SetRewardFactory(RewardFactory.Instance);
        Debug.Log("[EggPickup] 🏭 Set RewardFactory from singleton in Start()", this);
    }
}

    /* ------------- Lifecycle -------------- */
    private void OnEnable()
    {
        collected = false;
        Debug.Log("[EggPickup] ♻️ Enabled and reset.", this);

        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
            StartCoroutine(EnablePickupAfterDelay());
        }
    }

    private System.Collections.IEnumerator EnablePickupAfterDelay()
    {
        yield return new WaitForSeconds(pickupDelay);
        if (!collected && pickupCollider != null)
        {
            pickupCollider.enabled = true;
            Debug.Log("[EggPickup] ✅ Pickup collider enabled.", this);
        }
    }

    protected override void OnPickUp(GameObject player)
    {
        if (collected)
        {
            Debug.Log("[EggPickup] ❌ Already collected, skipping.", this);
            return;
        }
        collected = true;
        gameObject.SetActive(false);
        Debug.Log($"[EggPickup] 🥚 Collected by: {player.name}", this);
        SpawnSelectedReward();
    }

    /* ------------- Reward Logic ----------- */
    private void SpawnSelectedReward()
    {
        GameObject prefab = SelectRandomReward();
        if (prefab == null)
        {
            Debug.LogWarning("[EggPickup] ⚠️ No valid reward prefab found.", this);
            return;
        }
        if (rewardFactory == null)
        {
            Debug.LogError("[EggPickup] ❌ rewardFactory is null! Did you forget to call SetRewardFactory?", this);
            return;
        }
        rewardFactory.Spawn(prefab, transform.position);
        Debug.Log($"[EggPickup] 🎁 Spawned reward: {prefab.name} at {transform.position}", this);
    }

    private GameObject SelectRandomReward()
    {
        float total = 0f;
        foreach (var r in rewards) total += r.weight;
        if (total <= 0f)
        {
            Debug.LogWarning("[EggPickup] ❌ Total reward weight is 0.", this);
            return null;
        }

        float roll = Random.value * total;
        foreach (var r in rewards)
        {
            roll -= r.weight;
            if (roll <= 0f)
            {
                Debug.Log($"[EggPickup] 🎲 Selected: {r.prefab.name}", this);
                return r.prefab;
            }
        }
        Debug.LogWarning("[EggPickup] ❌ Failed to select reward.", this);
        return null;
    }

    /* ------------- IResettable ------------ */
    public void ResetState()
    {
        collected = false;
        gameObject.SetActive(true);
        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
            StartCoroutine(EnablePickupAfterDelay());
        }
        Debug.Log("[EggPickup] ♻️ ResetState called.", this);
    }
}
