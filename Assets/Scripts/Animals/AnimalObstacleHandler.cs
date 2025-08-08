using UnityEngine;

[RequireComponent(typeof(AnimalBase))]
public sealed class AnimalObstacleHandler : MonoBehaviour
{
    private AnimalBase animal;
    void Awake() => animal = GetComponent<AnimalBase>();

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1) ENEMIES: dismount if NOT invincible; NEVER kill here
        if (other.TryGetComponent<EnemyBase>(out var enemy))
        {
            bool riderInvincible = animal.Rider &&
                animal.Rider.GetComponentInChildren<IInvincible>()?.IsInvincible == true;

            if (!riderInvincible)
                animal.Rider?.GetComponent<RideController>()?.DismountCurrentAnimal();

            return; // <- Do not fall through to obstacle-destroy path
        }

        // 2) NON-ENEMY OBSTACLES: normal logic
        if (!other.TryGetComponent<IObstacle>(out var obstacle)) return;

        bool riderInv = animal.Rider &&
            animal.Rider.GetComponentInChildren<IInvincible>()?.IsInvincible == true;

        if (animal.Rider != null && !riderInv)
            animal.Rider.GetComponent<RideController>()?.DismountCurrentAnimal();

        // Extra guard: animals never "destroy" enemies through the obstacle path
        if (obstacle.Type != ObstacleType.Enemy && animal.CanDestroy(obstacle.Type))
            obstacle.DestroyObstacle();
    }
}
