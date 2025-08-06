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

        bool isInvincible = animal.Rider != null &&
                            animal.Rider.TryGetComponent<IInvincible>(out var inv) &&
                            inv.IsInvincible;

        if (animal.Rider != null && !isInvincible)
        {
            animal.Dismount();
        }

        if (animal.CanDestroy(obstacle.Type))
        {
            obstacle.DestroyObstacle();
        }
    }
}
