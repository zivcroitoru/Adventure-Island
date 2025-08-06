using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed = 6f;
    [SerializeField] protected int _damage = 1;

    protected IAttacker _attacker;

    // Generic pool reference via delegate
    private System.Action<BaseProjectile> _returnToPool;

    #region ───── Pooling ─────

    public void SetPool<T>(ProjectilePool<T> pool) where T : BaseProjectile
    {
        _returnToPool = bp => pool.Release((T)bp);
    }

    public void ReturnToPool()
    {
        if (_returnToPool == null)
        {
            Debug.LogError($"[BaseProjectile] Pool not set on {gameObject.name}");
            return;
        }

        _returnToPool.Invoke(this);
    }

    public virtual void OnSpawn()    { }
    public virtual void OnDespawn() { }

    #endregion

    #region ───── Initialization ─────

    public void SetAttacker(IAttacker attacker) => _attacker = attacker;

    #endregion

    #region ───── API ─────

    public abstract void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f);

    #endregion

    #region ───── Collision ─────

protected virtual void OnTriggerEnter2D(Collider2D other)
{
    ReturnToPool();   // no damage here
}



    #endregion
}
