using UnityEngine;

/// <summary>
/// When the mounted animal hits a Rock:
/// • Player is forced to dismount.
/// • The rock is destroyed.
/// Attach this to the Animal GameObject (needs a Collider2D set as Trigger).
/// </summary>
[RequireComponent(typeof(AnimalBase))]
public sealed class AnimalObstacleHandler : MonoBehaviour
{
    private AnimalBase animal;

    void Awake() => animal = GetComponent<AnimalBase>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IObstacle>(out var obstacle)) return;

        if (obstacle.Type == ObstacleType.Rock)
        {
            // Dismount rider (if present)
            animal.Rider?.GetComponent<RideController>()?.DismountCurrentAnimal();

            // Remove the rock
            obstacle.DestroyObstacle();
        }
    }
}
