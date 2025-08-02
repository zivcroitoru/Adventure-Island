using UnityEngine;

public class Rock : HazardBase 
{
    // No need to redefine `damage` here — inherited from HazardBase
    private void Reset()
    {
        damage = 3; // set default value for this hazard type
    }

    protected override bool TryHandleProjectileDestruction(Collider2D other)
    {
        if (other.GetComponent<ProjectileAxe>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
