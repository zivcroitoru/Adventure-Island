using UnityEngine;

public class ObstacleCollisionManager : MonoBehaviour
{
    [SerializeField] private GameObject rider; // Assigned from AnimalBase when mounted

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rider == null) return;

        var obstacle = collision.gameObject.GetComponent<IObstacle>();
        if (obstacle == null) return;

        // Apply damage to player via IDamageable
        if (obstacle.RidingDamage > 0)
        {
            var damageable = rider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(obstacle.RidingDamage);
            }
        }

        // Always destroy the obstacle and the animal when collided
        obstacle.DestroyObstacle();    // Destroys Rock / Fire
        Destroy(gameObject);          // This animal
    }
}
