using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable, IResettable, IObstacle
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    [Header("Enemy Settings")]
    [SerializeField] private EnemyType enemyType = EnemyType.Basic;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    // ── IObstacle interface ──
    public ObstacleType Type => ObstacleType.Enemy;
    public int ContactDamage => 999;
    public int RidingDamage => 999;
    public EnemyType EnemyType => enemyType;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;

        EnemyRespawnManager.Instance?.Register(this);
        GameResetManager.Instance?.Register(this);
    }

public virtual void TakeDamage(int amount)
{
    if (amount >= 1)  
    {
        Die();  
    }
}

protected void Die()
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

    public void DestroyObstacle() => TakeDamage(999);
}
