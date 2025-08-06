using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(DamageDealer))]
public abstract class HazardBase : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    protected virtual bool Filter(Collider2D other) => false;

private void OnTriggerEnter2D(Collider2D other)
{
    if (Filter(other)) return;

    // OLD:
    // var target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

    // NEW: Always resolve to the player if hitting an animal being ridden
    GameObject target;
    if (other.TryGetComponent<AnimalBase>(out var animal) && animal.Rider != null)
        target = animal.Rider;
    else
        target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

    Damage.Deal(damage, gameObject, target);
}

}
