using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(DamageDealer))]
public sealed class Rock : MonoBehaviour, IObstacle, IDamageable, IResettable
{
    public ObstacleType Type => ObstacleType.Rock;
    public int ContactDamage => 3;
    public int RidingDamage => 3;

    // Destroyable only by specific projectiles
    public void DestroyObstacle() => Destroy(gameObject);

    // Take damage, but check if the damage source is a valid projectile
    public void TakeDamage(int _)
    {
        // Check if the damage source is one of the valid projectiles
        if (TryGetComponent<ProjectileBoomerang>(out var boomerang) ||
            TryGetComponent<ProjectileSpark>(out var spark) ||
            TryGetComponent<ProjectileFire>(out var fire))
        {
            DestroyObstacle();
        }
    }

    public void ResetState() => gameObject.SetActive(true);
}
