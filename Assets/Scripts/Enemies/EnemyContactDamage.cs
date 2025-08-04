using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[EnemyContactDamage] Triggered by: {other.name}");

        if (other.TryGetComponent<RideController>(out var ride))
        {
            Debug.Log("[EnemyContactDamage] RideController found.");

            if (ride.IsRiding)
            {
                Debug.Log("[EnemyContactDamage] Player is riding.");

                if (ride.CurrentAnimal is GreenAnimal green && green.IsSpinning)
                {
                    Debug.Log("[EnemyContactDamage] Player is spinning. Enemy takes damage.");
                    DamageSelf();
                    return;
                }

                Debug.Log("[EnemyContactDamage] Player is not spinning. Forcing dismount.");
                ride.DismountCurrentAnimal();
                return;
            }
        }

        var damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log($"[EnemyContactDamage] Found IDamageable on: {other.name}. Applying {damage} damage.");
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"[EnemyContactDamage] No IDamageable found on {other.name} or its parents.");
        }
    }

    private void DamageSelf()
    {
        if (TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log("[EnemyContactDamage] Enemy self-damage triggered.");
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.Log("[EnemyContactDamage] Self is not IDamageable.");
        }
    }
}
