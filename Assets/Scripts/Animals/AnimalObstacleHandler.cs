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
        if (!other.TryGetComponent<IObstacle>(out var obstacle))
            return;

        bool canDestroy = animal.CanDestroy(obstacle.Type);
        bool isInvincible = animal.Rider != null &&
                            animal.Rider.TryGetComponent<IInvincible>(out var inv) &&
                            inv.IsInvincible;

        // Dismount only if not invincible
        if (animal.Rider != null && !isInvincible)
        {
            animal.Dismount();
        }

        // Destroy the obstacle only if the animal is allowed
        if (canDestroy)
        {
            obstacle.DestroyObstacle();
        }
    }
}
