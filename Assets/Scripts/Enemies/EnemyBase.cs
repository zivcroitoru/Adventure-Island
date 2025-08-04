using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public virtual void TakeDamage(int amount)
    {
        Debug.Log("[EnemyBase] Took damage. Destroying.");
        Destroy(gameObject);
    }
}
