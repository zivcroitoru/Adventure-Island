using UnityEngine;

[RequireComponent(typeof(AnimalBase))]
public sealed class AnimalObstacleHandler : MonoBehaviour
{
    private AnimalBase animal;

    void Awake() => animal = GetComponent<AnimalBase>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IObstacle>(out var obstacle)) return;

        // Always dismount if there's a rider, but skip if the rider is invincible
        if (animal.Rider != null)
        {
            // Check if the rider is invincible before dismounting
            if (!animal.Rider.TryGetComponent<IInvincible>(out var invincible) || !invincible.IsInvincible)
            {
                animal.Rider.GetComponent<RideController>()?.DismountCurrentAnimal();
            }
            else
            {
                Debug.Log("[AnimalObstacleHandler] Rider is invincible, skipping dismount.");
            }
        }

        // Destroy the obstacle only if the animal is allowed to
        if (animal.CanDestroy(obstacle.Type))
            obstacle.DestroyObstacle();
    }
}
