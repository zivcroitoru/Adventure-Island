using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class DamageDealer : MonoBehaviour
{
    [SerializeField] int fallbackDamage = 20;
    [SerializeField] bool destroyAfterHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTarget(other);
        int amount = CalculateDamage(target);

        Damage.Deal(amount, gameObject, target);

        if (destroyAfterHit && !TryGetComponent<IObstacle>(out _))
            Destroy(gameObject);
    }

    private GameObject ResolveTarget(Collider2D hit)
    {
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
            return animal.Rider;

        return hit.attachedRigidbody ? hit.attachedRigidbody.gameObject : hit.gameObject;
    }

    private int CalculateDamage(GameObject target)
    {
        if (TryGetComponent<IObstacle>(out var obs))
        {
            bool isRiding = target.TryGetComponent<RideController>(out var rc) && rc.IsRiding;
            return isRiding ? obs.RidingDamage : obs.ContactDamage;
        }

        return fallbackDamage;
    }
}
