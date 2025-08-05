using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed = 6f;
    [SerializeField] protected int _damage = 1;

    protected IAttacker _attacker;
protected System.Action<BaseProjectile> _returnToPool;

    /* ---------- Initialization ---------- */

    public void InitPool<T>(ObjectPool<T> pool) where T : BaseProjectile
    {
        // Store a safe despawn delegate — no casting issues
        _returnToPool = (BaseProjectile p) => pool.Despawn(p as T);
    }

    public void SetAttacker(IAttacker attacker)
    {
        _attacker = attacker;
    }

    /* ---------- Shooting API ---------- */

    public abstract void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f);

    /* ---------- Poolable ---------- */

    public virtual void ResetState() { }

    public virtual void OnSpawn() { }

    /* ---------- Collision ---------- */

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
            target.TakeDamage(_damage);

        _returnToPool?.Invoke(this);
    }
}
