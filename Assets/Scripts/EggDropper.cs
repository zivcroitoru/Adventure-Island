using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class EggDropper : MonoBehaviour
{
    [SerializeField] private EggPickup eggPrefab;

    [Inject] private IObjectResolver resolver;

    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        if (enemy != null)
            enemy.OnDeath += OnEnemyDeath;
    }

private void OnEnemyDeath(EnemyBase _, Vector3 pos, Quaternion rot)
{
    if (eggPrefab == null)
    {
        Debug.LogWarning("[EggDropper] ❌ No egg prefab assigned.");
        return;
    }

    GameObject eggGO = Instantiate(eggPrefab.gameObject, pos, rot);

    if (eggGO.TryGetComponent(out EggPickup eggPickup))
    {
        eggPickup.SetResolver(resolver);
        Debug.Log("[EggDropper] ✅ Resolver injected into dropped egg.");
    }
    else
    {
        Debug.LogError("[EggDropper] ❌ Instantiated egg missing EggPickup component.");
    }
}



    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnDeath -= OnEnemyDeath;
    }
}
