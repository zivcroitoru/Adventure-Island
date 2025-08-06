using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable, IResettable, IObstacle
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    // --- IObstacle values ---
    public ObstacleType Type => ObstacleType.Enemy;
    public int ContactDamage => 999;  // or higher depending on enemy type
    public int RidingDamage  => 999;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;

        EnemyRespawnManager.Instance?.Register(this);
        GameResetManager.Instance?.Register(this);
    }

    public virtual void TakeDamage(int amount)
    {
        OnDeath?.Invoke(this, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public virtual void ResetState()
    {
        gameObject.SetActive(true);
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;
    }

    public Vector3 GetSpawnPosition() => _spawnPos;
    public Quaternion GetSpawnRotation() => _spawnRot;

    public void DestroyObstacle()
    {
        TakeDamage(999); // or just gameObject.SetActive(false);
    }
}
