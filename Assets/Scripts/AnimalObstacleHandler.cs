using UnityEngine;

[RequireComponent(typeof(AnimalBase))]
public class AnimalObstacleHandler : MonoBehaviour
{
    private AnimalBase animal;

    private void Awake() => animal = GetComponent<AnimalBase>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var obstacle = other.GetComponent<IObstacle>();
        if (obstacle == null) return;            // not a Rock / Fire / …

        // 1. Hurt the rider if needed
        if (animal.Rider != null && obstacle.RidingDamage > 0)
        {
            var dmg = animal.Rider.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(obstacle.RidingDamage);  // EnergyController handles this
        }

        // 2. Destroy animal and obstacle
        if (animal.Rider != null) Destroy(gameObject);   // the animal
        obstacle.DestroyObstacle();                      // the rock / fire
    }
}
