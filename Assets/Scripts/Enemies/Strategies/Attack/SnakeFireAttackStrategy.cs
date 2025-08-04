using UnityEngine;

public class SnakeFireAttackStrategy : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private SnakeFireProjectilePoolManager poolManager;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Vector2 fireDirection = Vector2.left;

    private float _nextFireTime;

    public void Attack()
    {
        if (Time.time < _nextFireTime || poolManager == null)
            return;

        SnakeFireProjectile projectile = poolManager.GetPooledProjectile();
        if (projectile == null)
            return;

        projectile.Shoot(transform.position, fireDirection);
        _nextFireTime = Time.time + fireCooldown;
    }
}
