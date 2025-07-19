using System.Collections;
using UnityEngine;

// Base class for all projectiles (like lasers or fireballs)
public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float _speed;         // How fast it moves
    [SerializeField] protected float _destroyTime;   // How long it stays active

    // Stops any timers when the projectile gets turned off
    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        Debug.Log("[Laser] Disabled (pooled)");
    }

    // Set speed and lifetime from the outside (usually by the builder)
    public virtual void Initialized(float speed, float lifeTime)
    {
        _speed = speed;
        _destroyTime = lifeTime;
    }

    // Every projectile needs to define how it shoots
    public abstract void Shoot();

    // Starts the timer that turns the projectile off
    protected void StartDeactivateTimer()
    {
        StartCoroutine(DeactivateLogic());
    }

    // Waits for a bit, then disables the projectile
    private IEnumerator DeactivateLogic()
    {
        yield return new WaitForSeconds(_destroyTime);
        StopAllCoroutines();
        gameObject.SetActive(false); // Just turn off instead of destroying
    }

    // If the projectile hits something
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ShadowEnemy>(out var enemy))
        {
            HandleEnemyHit(enemy);
        }
    }

    // What to do when we hit an enemy
    protected virtual void HandleEnemyHit(ShadowEnemy enemy)
    {
        Debug.Log($"[Laser Hit] Hit enemy at: {transform.position}");
        Destroy(enemy.gameObject);     // Remove the enemy
        gameObject.SetActive(false);   // Turn off the projectile
    }
}
