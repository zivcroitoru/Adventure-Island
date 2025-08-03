using UnityEngine;

[RequireComponent(typeof(AnimalBase))]
public class AnimalObstacleHandler : MonoBehaviour
{
    private AnimalBase animal;

    private void Awake()
    {
        animal = GetComponent<AnimalBase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IObstacle obstacle))
            return;

        if (animal.Rider != null)
        {
            HandleRiderDamage(obstacle);
            ForceDismount(); // simplified
        }

        obstacle.DestroyObstacle();
    }

    private void HandleRiderDamage(IObstacle obstacle)
    {
        if (obstacle.RidingDamage <= 0) return;

        if (animal.Rider.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(obstacle.RidingDamage);
        }
    }

    private void ForceDismount()
    {
        if (animal.Rider.TryGetComponent(out RideController ride))
        {
            ride.UnmountAnimal(); // uses your clean method
        }
    }
}
