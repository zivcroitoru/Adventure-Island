using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for all projectile behaviors.
/// Handles movement speed, lifetime, and deactivation logic.
/// </summary>
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _destroyTime;

    private Coroutine _deactivateCoroutine;

    protected virtual void OnDisable()
    {
        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);
            _deactivateCoroutine = null;
        }

        Debug.Log("[BaseProjectile] Disabled (pooled)");
    }

    /// <summary>
    /// Called when the projectile is created or retrieved from pool.
    /// </summary>
    public virtual void Initialized(float speed, float lifeTime)
    {
        _speed = speed;
        _destroyTime = lifeTime;

        Debug.Log($"[BaseProjectile] Initialized with speed={_speed}, destroyTime={_destroyTime}");
    }

    public abstract void Shoot();

    protected void StartDeactivateTimer()
    {
        if (_destroyTime <= 0)
        {
            Debug.LogWarning("[BaseProjectile] Destroy time is 0 or negative! Disabling immediately.");
            gameObject.SetActive(false);
            return;
        }

        Debug.Log($"[BaseProjectile] Starting deactivate timer for {_destroyTime} seconds");
        _deactivateCoroutine = StartCoroutine(DeactivateLogic());
    }

    private IEnumerator DeactivateLogic()
    {
        yield return new WaitForSeconds(_destroyTime);
        Debug.Log("[BaseProjectile] Deactivation time reached — disabling");
        gameObject.SetActive(false);
    }
}
