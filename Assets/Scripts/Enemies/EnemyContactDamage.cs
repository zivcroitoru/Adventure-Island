using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        if (target.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
        {
            TryDamageSelf();
            return;
        }

        if (target.TryGetComponent<RideController>(out var ride))
        {
            if (ride.IsRiding)
            {
                if (ride.CurrentAnimal is GreenAnimal green && green.IsSpinning)
                {
                    TryDamageSelf();
                    return;
                }

                ride.DismountCurrentAnimal();
                return;
            }
        }

        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    private void TryDamageSelf()
    {
        if (TryGetComponent<IDamageable>(out var self))
        {
            self.TakeDamage(damage);
        }
    }
}
