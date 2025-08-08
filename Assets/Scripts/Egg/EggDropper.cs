using UnityEngine;

[DisallowMultipleComponent]
public sealed class EggDropper : MonoBehaviour, IResettable
{
    [SerializeField] private EggPickup eggPrefab;

    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
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
    private void OnEnable()
    {
        if (enemy != null) enemy.OnDeath += OnEnemyDeath;

    }

    private void OnDisable()
    {
        if (enemy != null) enemy.OnDeath -= OnEnemyDeath;

    }

    public void ResetState()
    {
        // If you need to reset the egg (for example, in a pool system),
        // we can reactivate it and reset the reward state flag.
        gameObject.SetActive(true);
    }

}