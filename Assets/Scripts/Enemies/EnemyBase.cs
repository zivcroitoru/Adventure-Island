using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable, IResettable, IObstacle
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    [Header("Enemy Settings")]
    [SerializeField] private EnemyType enemyType = EnemyType.Basic;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private bool respawnWithFullHealth = true;

    [Header("Obstacle (contact)")]
    [SerializeField] private int contactDamage = 999;
    [SerializeField] private int ridingDamage  = 999;
    [SerializeField] private ObstacleType obstacleType = ObstacleType.Enemy;

    private int _health;
    private bool _dead;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    /* ───────── IObstacle ───────── */
    public ObstacleType Type => obstacleType;
    public int ContactDamage => contactDamage;
    public int RidingDamage  => ridingDamage;
    public EnemyType EnemyType => enemyType;

    public bool IsDead => _dead;
    public int Health => _health;
    public int MaxHealth => maxHealth;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;

        _health = Mathf.Max(1, maxHealth);
        _dead = false;

        EnemyRespawnManager.Instance?.Register(this);
        GameResetManager.Instance?.Register(this);
    }

public virtual void TakeDamage(int amount, GameObject dealer)
{
    ApplyDamage(amount);
}

public virtual void TakeDamage(int amount)
{
    ApplyDamage(amount);
}


    protected virtual void ApplyDamage(int amount)
    {
        if (_dead || amount <= 0) return;

        _health -= amount;
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        if (_dead) return;
        _dead = true;

        OnDeath?.Invoke(this, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    /* ───────── IObstacle ───────── */
    public void DestroyObstacle()
    {
        // Treat as lethal hit
        ApplyDamage(int.MaxValue);
    }

    /* ───────── IResettable ───────── */
    public virtual void ResetState()
    {
        gameObject.SetActive(true);
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;

        _dead = false;
        _health = respawnWithFullHealth ? Mathf.Max(1, maxHealth) : _health;
    }

    public Vector3 GetSpawnPosition() => _spawnPos;
    public Quaternion GetSpawnRotation() => _spawnRot;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (maxHealth < 1) maxHealth = 1;
        if (contactDamage < 0) contactDamage = 0;
        if (ridingDamage < 0) ridingDamage = 0;
    }
#endif
}
