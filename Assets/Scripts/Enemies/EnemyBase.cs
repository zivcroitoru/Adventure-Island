using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable, IResettable
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;
        Debug.Log($"[EnemyBase] Awake: Spawn position set to {_spawnPos}");

        if (EnemyRespawnManager.Instance == null)
            Debug.LogWarning("[EnemyBase] Awake: EnemyRespawnManager instance is NULL");
        else
            EnemyRespawnManager.Instance.Register(this);

        GameResetManager.Instance?.Register(this);
    }

    public virtual void TakeDamage(int amount)
    {
        Debug.Log($"[EnemyBase] Took {amount} damage at position {transform.position}");

        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        Debug.Log("[EnemyBase] Broadcasting death event");
        OnDeath?.Invoke(this, currentPos, currentRot);

        // If using reset system, comment this out:
        // EnemyRespawnManager.Instance?.RequestRespawn(this, currentPos, currentRot);

        Debug.Log("[EnemyBase] Disabling enemy GameObject");
        gameObject.SetActive(false);
    }

    public virtual void ResetState()
    {
        Debug.Log("[EnemyBase] ResetState called: resetting enemy.");

        gameObject.SetActive(true);
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;

        // Reset health or other necessary state here
    }

    public Vector3 GetSpawnPosition() => _spawnPos;
    public Quaternion GetSpawnRotation() => _spawnRot;
}
