using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;
        EnemyRespawnManager.Instance?.Register(this);
    }

public virtual void TakeDamage(int amount)
{
    Debug.Log("[EnemyBase] 💢 Took damage → broadcasting death, then disabling.");

    // Use *current* position and rotation
    Vector3 currentPos = transform.position;
    Quaternion currentRot = transform.rotation;

    OnDeath?.Invoke(this, currentPos, currentRot);

    // Tell global respawn manager to handle this enemy
    EnemyRespawnManager.Instance?.RequestRespawn(this, currentPos, currentRot);

    gameObject.SetActive(false);
}


    public Vector3 GetSpawnPosition() => _spawnPos;
    public Quaternion GetSpawnRotation() => _spawnRot;
}
