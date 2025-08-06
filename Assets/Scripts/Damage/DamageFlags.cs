using UnityEngine;

[System.Flags]
public enum DamageFlags
{
    None       = 0,
    Invincible = 1 << 0,
    Riding     = 1 << 1
}

/// <summary>
/// Passed to Damage.Deal so the rule table knows who hit whom
/// </summary>
public struct DamageContext
{
    public GameObject Dealer;   // hazard / enemy / projectile …
    public GameObject Target;   // player / crate / animal …
    public int Amount;          // damage value
    public DamageFlags Flags;   // Invincible | Riding | …
}
