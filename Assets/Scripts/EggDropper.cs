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

        var egg = Instantiate(eggPrefab, pos, Quaternion.identity);
        resolver.InjectGameObject(egg.gameObject);
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnDeath -= OnEnemyDeath;
    }
}
