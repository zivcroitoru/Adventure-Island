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
    Debug.Log("OnEnemyDeath triggered");

    if (eggPrefab == null)
    {
        Debug.Log("EggPrefab is null");
        return;
    }
    
    if (RewardFactory.Instance == null)
    {
        Debug.Log("RewardFactory is null");
        return;
    }

    Debug.Log("enemy died - egg drop");
    
    // Spawn egg and wire up its RewardFactory reference
    EggPickup egg = Instantiate(eggPrefab, pos, rot);
    Debug.Log($"Egg spawned at {pos} with rotation {rot}");
}


    private void OnDestroy()
    {
        if (enemy != null) enemy.OnDeath -= OnEnemyDeath;
    }
}
