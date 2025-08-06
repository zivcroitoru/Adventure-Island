using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 1;

private void OnTriggerEnter2D(Collider2D other)
{
    // IGNORE: if *this* is a projectile and the thing we hit is an obstacle
    if (TryGetComponent<BaseProjectile>(out _) &&
        other.TryGetComponent<IObstacle>(out _))
        return;

    Damage.Deal(damage, gameObject, other.gameObject);
}

}
