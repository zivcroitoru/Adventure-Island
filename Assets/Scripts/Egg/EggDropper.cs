using UnityEngine;

[DisallowMultipleComponent]
public sealed class EggDropper : MonoBehaviour
{
    [SerializeField] private EggPickup eggPrefab;

    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        if (enemy != null) enemy.OnDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(EnemyBase _, Vector3 pos, Quaternion rot)
    {
        if (eggPrefab == null || RewardFactory.Instance == null) return;

        // Spawn egg and wire up its RewardFactory reference
        EggPickup egg = Instantiate(eggPrefab, pos, rot);
        egg.SetRewardFactory(RewardFactory.Instance);
    }

    private void OnDestroy()
    {
        if (enemy != null) enemy.OnDeath -= OnEnemyDeath;
    }
}
