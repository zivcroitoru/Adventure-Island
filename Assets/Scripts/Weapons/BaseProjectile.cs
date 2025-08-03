using UnityEngine;

/// <summary>
/// Base class for all projectile behaviors.
/// Handles common initialization and damage interaction.
/// </summary>
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed;

    /// <summary>
    /// Called when the projectile is disabled (returned to pool or removed).
    /// </summary>
    protected virtual void OnDisable()
    {
        Debug.Log("[BaseProjectile] Disabled (pooled)");
    }

    /// <summary>
    /// Called when the projectile is first created or reset.
    /// </summary>
    public virtual void Initialized(float speed)
    {
        _speed = speed;
        Debug.Log($"[BaseProjectile] Initialized with speed={_speed}");
    }

    /// <summary>
    /// Launch behavior, implemented by subclasses.
    /// </summary>
    public abstract void Shoot();

    /// <summary>
    /// Handles contact with IDamageable targets.
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            Debug.Log($"[BaseProjectile] Hit {other.name} — applying damage.");
            target.TakeDamage(1);
        }
    }
}
