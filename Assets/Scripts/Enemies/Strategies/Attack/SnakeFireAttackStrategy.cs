using UnityEngine;
using VContainer;

public class SnakeFireAttackStrategy : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Vector2 fireDirection = Vector2.left;

    private SnakeFireProjectilePool _pool;
    private float _nextFireTime;
private void Awake() {
    Debug.Log("[SnakeFireAttack] Awake called on: " + gameObject.name);
}


    [Inject]
    public void Construct(SnakeFireProjectilePool pool)
    {
        _pool = pool;
        Debug.Log("[SnakeFireAttack] Pool injected.");
    }

    public void Attack()
    {
        if (Time.time < _nextFireTime)
        {
            Debug.Log($"[SnakeFireAttack] Cooldown active. Next fire at {Mathf.Round(_nextFireTime - Time.time)}s.");
            return;
        }

        if (_pool == null)
        {
            // Debug.LogWarning("[SnakeFireAttack] Pool is null. Injection might have failed.");
            return;
        }

        var projectile = _pool.Get(transform.position, Quaternion.identity);
        if (projectile == null)
        {
            Debug.LogWarning("[SnakeFireAttack] No projectile returned from pool.");
            return;
        }

        Debug.Log("[SnakeFireAttack] 🔥 Shooting fire projectile.");
        projectile.Shoot(transform.position, fireDirection);
        _nextFireTime = Time.time + fireCooldown;
    }
}
