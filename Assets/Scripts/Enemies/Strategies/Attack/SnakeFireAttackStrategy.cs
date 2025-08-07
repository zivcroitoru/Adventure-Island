using UnityEngine;
using VContainer;

public class SnakeFireAttackStrategy : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Vector2 fireDirection = Vector2.left;

    private SnakeFireProjectilePool _pool;
    private float _nextFireTime;

    [Inject]
    public void Construct(SnakeFireProjectilePool pool)
    {
        _pool = pool;
    }

    public void Attack()
    {
        // Prevent firing if cooldown is active or pool not ready
        if (Time.time < _nextFireTime || _pool == null)
            return;

        var projectile = _pool.Get(transform.position, Quaternion.identity);
        if (projectile == null)
            return;

        projectile.Shoot(transform.position, fireDirection);
        _nextFireTime = Time.time + fireCooldown;
    }
}
