using UnityEngine;

public class SnakeFireAttackStrategy : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private SnakeFireProjectilePool projectilePool;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Vector2 fireDirection = Vector2.left;

    private float _nextFireTime;

    public void Attack()
    {
        if (Time.time < _nextFireTime || projectilePool == null)
            return;

        var projectile = projectilePool.Get(transform.position, Quaternion.identity);
        if (projectile == null)
            return;

        projectile.Shoot(transform.position, fireDirection);
        _nextFireTime = Time.time + fireCooldown;
    }
}
