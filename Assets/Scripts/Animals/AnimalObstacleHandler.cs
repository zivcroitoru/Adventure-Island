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
        if (!other.TryGetComponent(out IObstacle obstacle)) return;

        if (animal.Rider != null)
        {
            // If the animal can't destroy it, dismount only
            if (!animal.CanDestroy(obstacle.Type))
            {
                animal.Dismount();
                return;
            }

            // If it can destroy, dismount and break
            animal.Dismount();
        }

        if (animal.CanDestroy(obstacle.Type))
        {
            obstacle.DestroyObstacle();
        }
    }
}
