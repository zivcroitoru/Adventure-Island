using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileAxe : MonoBehaviour, IPoolable
{
    private ProjectilePool<ProjectileAxe> _pool;

    public void Init(ProjectilePool<ProjectileAxe> pool)
    {
        _pool = pool;
    }

    public void OnSpawn()
    {
        // Reset velocity, animations, trail, etc.
        // Example: GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void OnDespawn()
    {
        // Clean up effects, disable trail, reset state, etc.
    }

    public void ReturnToPool()
    {
        if (_pool != null)
            _pool.Release(this);
        else
            Debug.LogWarning("[ProjectileAxe] Tried to return to pool, but pool is null.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }
}
