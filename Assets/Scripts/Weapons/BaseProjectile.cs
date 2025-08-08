using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed = 6f;
    [SerializeField] protected int _damage = 1;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Transform ownerRoot;

    Action<BaseProjectile> _returnToPool;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>Called by shooters. Needed to ignore self-hits.</summary>
    public void SetOwner(Transform owner) => ownerRoot = owner ? owner.root : null;

    /// <summary>Common "shoot" hook so derived classes can override.</summary>
    public virtual void Shoot(Vector2 origin, Vector2 direction, float speed)
    {
        transform.position = origin;
        rb.velocity = direction.normalized * speed;
    }

    /// <summary>Base hook so derived OnTriggerEnter2D overrides compile.</summary>
// BaseProjectile.cs
protected virtual void OnTriggerEnter2D(Collider2D other)
{
    if (HitIsOwner(other.gameObject)) return;
    HandleHit(other); // single place for shared logic
}

/// <summary>Default hit behavior: damage IDamageable.</summary>
protected virtual void HandleHit(Collider2D other)
{
    if (other.TryGetComponent<IDamageable>(out var d))
        d.TakeDamage(_damage, gameObject);
}



    /// <summary>Utility for derived classes when handling hits.</summary>
    protected bool HitIsOwner(GameObject target) =>
        ownerRoot && target && target.transform.root == ownerRoot;

    #region Pool API
    public void SetPool<T>(ProjectilePool<T> pool) where T : BaseProjectile
    {
        _returnToPool = p => pool.Release((T)p);
    }

    public void ReturnToPool()
    {
        if (_returnToPool != null) _returnToPool(this);
        else Destroy(gameObject);
    }

    public virtual void OnSpawn() { }
    public virtual void OnDespawn() { }
    #endregion
}
