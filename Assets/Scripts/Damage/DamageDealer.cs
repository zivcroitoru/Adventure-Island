// DamageDealer.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class DamageDealer : MonoBehaviour
{
    [SerializeField] int fallbackDamage = 20;     // when not an obstacle
    [SerializeField] bool destroyAfterHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTarget(other);       // player, crate, etc.
        int amount = CalcDamage(target);         // contact vs riding
        Damage.Deal(amount, gameObject, target); // rule table

        if (destroyAfterHit && !TryGetComponent<IObstacle>(out _))
            Destroy(gameObject);
    }

    // Redirect to rider if we hit their mount
    static GameObject ResolveTarget(Collider2D hit)
    {
        if (hit.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
            return animal.Rider;

        return hit.attachedRigidbody ? hit.attachedRigidbody.gameObject
                                     : hit.gameObject;
    }

    // Obstacles carry per-mode damage; everything else uses fallback
    int CalcDamage(GameObject target)
    {
        if (TryGetComponent<IObstacle>(out var obs))
        {
            bool riding = target.TryGetComponent<RideController>(out var rc) && rc.IsRiding;
            return riding ? obs.RidingDamage : obs.ContactDamage;
        }
        return fallbackDamage;
    }
}
