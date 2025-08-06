// Rock.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(DamageDealer))]
public sealed class Rock : MonoBehaviour, IObstacle, IDamageable
{
    public ObstacleType Type => ObstacleType.Rock;
    public int ContactDamage => 3;
    public int RidingDamage  => 3;

    public void DestroyObstacle() => Destroy(gameObject);
    public void TakeDamage(int _) => DestroyObstacle();
}
