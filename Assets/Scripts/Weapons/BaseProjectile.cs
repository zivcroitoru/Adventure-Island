using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed = 6f;
    [SerializeField] protected int _damage = 1;

    protected IAttacker _attacker;
    private ProjectilePool<BaseProjectile> _pool;

    /* ---------- Pool Setup ---------- */
public void SetPool<T>(ProjectilePool<T> pool) where T : MonoBehaviour, IPoolable
{
    _pool = pool as ProjectilePool<BaseProjectile>;
}


    public virtual void OnSpawn() { }
    public virtual void OnDespawn() { }
    public void ReturnToPool() => _pool?.Release(this);

    /* ---------- Setup ---------- */
    public void SetAttacker(IAttacker attacker) => _attacker = attacker;

    /* ---------- API ---------- */
    public abstract void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f);

    /* ---------- Collision ---------- */
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
            target.TakeDamage(_damage);

        ReturnToPool();
    }
}
