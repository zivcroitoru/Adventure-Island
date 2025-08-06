using UnityEngine;

public class Fire : HazardBase, IObstacle, IDamageable
{
    public ObstacleType Type => ObstacleType.Fire;
    public int ContactDamage => 999;
    public int RidingDamage => 0;

    public void TakeDamage(int amount)
    {
        DestroyObstacle();
    }

    public void DestroyObstacle()
    {
        Debug.Log("[Fire] 🔥 Destroyed!");
        Destroy(gameObject);
    }

    protected override bool HandleExternalDestruction(Collider2D other)
    {
        if (other.TryGetComponent<BaseProjectile>(out _))
        {
            Debug.Log("[Fire] 🔥 Ignoring projectile hit.");
            return true;
        }

        return false;
    }


}
