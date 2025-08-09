using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class InvincibleTouchKiller : MonoBehaviour
{
    [SerializeField] int lethalDamage = 999999;

    void OnTriggerEnter2D(Collider2D other)
    {
        // find IInvincible on self or parents
        if (!(TryGetComponent<IInvincible>(out var invincible) && invincible.IsInvincible))
            return;

        GameObject hit = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (hit.transform.root == transform.root) return;

        if (hit.TryGetComponent<IObstacle>(out var obstacle))
            obstacle.DestroyObstacle();
        else if (hit.TryGetComponent<IDamageable>(out var dmg) || (dmg = hit.GetComponentInParent<IDamageable>()) != null)
            dmg.TakeDamage(lethalDamage, gameObject);
    }
}



