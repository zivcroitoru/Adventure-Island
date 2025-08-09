using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed  = 6f;     // default launch speed
    [SerializeField] protected int   _damage = 1;      // contact damage
    [SerializeField] private bool    _despawnOnHit = true; // auto-return after a hit (boomerang overrides)

    // --- Cached components ---
    [NonSerialized] private Rigidbody2D    _rb;
    [NonSerialized] private SpriteRenderer _sr;
    protected Rigidbody2D    Rb => _rb ??= GetComponent<Rigidbody2D>();
    protected SpriteRenderer Sr => _sr ??= GetComponent<SpriteRenderer>();

    // --- Ownership (ignore self/owner root) ---
    protected Transform ownerRoot;
    public void SetOwner(Transform owner) => ownerRoot = owner ? owner.root : null;

    // --- Pool wiring & guards ---
    private Action<BaseProjectile> _releaseToPool;
    private bool _isInPool;
    private bool _isReleasing;
    private bool _isDespawning;
    private bool _hitThisActivation;

    // ----------------- Lifecycle -----------------
    public virtual void OnSpawn()
    {
        _isInPool = false;
        _isReleasing = false;
        _isDespawning = false;
        _hitThisActivation = false;
        if (_rb) _rb.velocity = Vector2.zero;
        // keep ownerRoot unless the shooter resets it per shot
    }

    public virtual void OnDespawn()
    {
        if (_isDespawning) return;
        _isDespawning = true;

        if (_rb) _rb.velocity = Vector2.zero;
        _hitThisActivation = false;
        ownerRoot = null;

        _isDespawning = false;
    }

    // ----------------- Firing -----------------
    /// Sets position & velocity (direction normalized if needed).
    public virtual void Shoot(Vector2 origin, Vector2 direction, float speed)
    {
        transform.position = origin;
        var dir = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
        Rb.velocity = dir * speed;
    }

    // ----------------- Collision -----------------
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_hitThisActivation || _isReleasing || _isInPool) return;

        var hitGO = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (IsSelf(hitGO)) return;

        HandleHit(hitGO);
    }

    /// Centralized damage path; base optionally despawns after the hit.
    protected virtual void HandleHit(GameObject hitGO)
    {
        Damage.Deal(_damage, gameObject, hitGO);

        if (_despawnOnHit)
        {
            _hitThisActivation = true; // avoid multi-hit same activation/frame
            ReturnToPool();
        }
    }

    protected bool IsSelf(GameObject target) =>
        ownerRoot && target && target.transform.root == ownerRoot;

    // ----------------- Pool API -----------------
    /// Wire once by the spawner/weapon: proj.SetPool(pool.Release)
    public void SetPool(Action<BaseProjectile> releaseToPool)
    {
        _releaseToPool = releaseToPool;
    }
public void SetPool<T>(ProjectilePool<T> pool) where T : BaseProjectile
{
    // Wire a delegate that releases this projectile back to its typed pool
    SetPool(bp => pool.Release((T)bp));
}


    /// Gameplay asks to return to pool; idempotent & re-entrancy safe.
    public void ReturnToPool()
    {
        if (_isInPool || _isReleasing) return;
        _isReleasing = true;

        if (_releaseToPool != null)
            _releaseToPool(this);
        else
            Destroy(gameObject); // non-pooled fallback

        _isInPool = true;
        _isReleasing = false;
    }
}
