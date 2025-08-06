using UnityEngine;
using VContainer;
using VContainer.Unity;

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

    private IObjectResolver _resolver;
    private bool _collected;

    private void Start()
    {
        if (_resolver == null)
        {
            var scope = FindFirstObjectByType<LifetimeScope>();
            if (scope != null)
                _resolver = scope.Container;
        }

        GameResetManager.Instance?.Register(this);
    }

    public void SetResolver(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    protected override void OnPickUp(GameObject player)
    {
        if (_collected) return;
        _collected = true;

        gameObject.SetActive(false);

        var prefab = SelectReward();
        if (prefab == null) return;

        var reward = Instantiate(prefab, transform.position, Quaternion.identity);
        _resolver?.InjectGameObject(reward);
    }

    private GameObject SelectReward()
    {
        float total = 0f;
        foreach (var r in rewards) total += r.weight;

        float roll = Random.value * total;
        foreach (var r in rewards)
        {
            if ((roll -= r.weight) <= 0f)
                return r.prefab;
        }

        return null;
    }

    public void ResetState()
    {
        _collected = false;
        gameObject.SetActive(true);
    }
}
