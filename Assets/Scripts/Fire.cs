using UnityEngine;

public class Fire : HazardBase
{
    private void Reset()
    {
        damage = 20;
    }

    protected override void TryApplyDamage(Collider2D other)
    {
        var invincible = other.GetComponent<IInvincible>();
        if (invincible != null && invincible.IsInvincible)
        {
            Debug.Log("[Fire] Player is invincible — destroying fire.");
            Destroy(gameObject); // destroy the fire
            return;
        }

        base.TryApplyDamage(other); // fallback to normal damage
    }
}
