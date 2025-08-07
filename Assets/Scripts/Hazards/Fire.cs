using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(DamageDealer))]
public sealed class Fire : MonoBehaviour, IObstacle, IResettable
{
    public ObstacleType Type => ObstacleType.Fire;
    public int ContactDamage => 999;
    public int RidingDamage => 0;

    // Destroy the fire only if the player is invincible
    public void DestroyObstacle()
    {
        // Check if the object hitting the fire has an IInvincible component and is invincible
        GameObject rider = FindObjectOfType<AnimalBase>().Rider;  // Adjust if you have a direct reference to the rider or player
        if (rider != null && rider.TryGetComponent<IInvincible>(out var invincibleComponent) && invincibleComponent.IsInvincible)
        {
            Debug.Log("[Fire] Fire destroyed because the player is invincible.");
            Destroy(gameObject);  // Destroy the fire if the rider is invincible
        }
        else
        {
            Debug.Log("[Fire] Fire cannot be destroyed because the player is not invincible.");
        }
    }

    public void ResetState() => gameObject.SetActive(true);
}
