// Fire.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(DamageDealer))]
public sealed class Fire : MonoBehaviour, IObstacle         // ⚠️ note: no IDamageable
{
    public ObstacleType Type => ObstacleType.Fire;

    // Huge damage on foot, none while riding (you auto-dismount instead)
    public int ContactDamage => 999;
    public int RidingDamage  => 0;

    public void DestroyObstacle() => Destroy(gameObject);    // called only by invincible rule
}
