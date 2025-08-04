using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player has a RideController and is riding, dismount instead of damaging
        if (other.TryGetComponent<RideController>(out var ride) && ride.IsRiding)
        {
            Debug.Log("[EnemyContactDamage] Player is riding — forcing dismount instead of damage.");
            ride.DismountCurrentAnimal(); // ✅ dismounts safely
            return;
        }

        // Otherwise apply damage
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(contactDamage);
            Debug.Log($"[EnemyContactDamage] Damaged {other.name} for {contactDamage}");
        }
    }
}
