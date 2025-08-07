using UnityEngine;
using VContainer;        // VContainer injection
using VContainer.Unity;  // IObjectResolver

/// <summary>
/// Spawns reward prefabs with VContainer-injection already applied.
/// </summary>
[DisallowMultipleComponent]
public sealed class RewardFactory : MonoBehaviour
{
    public static RewardFactory Instance { get; private set; }

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

    // VContainer calls this automatically
    [Inject]
    public void Construct(IObjectResolver resolver) => _resolver = resolver;

    /// <summary>Instantiate a prefab at <paramref name="position"/> with all
    /// dependencies injected. Returns the spawned object.</summary>
    public GameObject Spawn(GameObject prefab, Vector3 position) =>
        _resolver.Instantiate(prefab, position, Quaternion.identity);
}
