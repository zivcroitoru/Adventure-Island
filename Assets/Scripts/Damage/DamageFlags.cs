using UnityEngine;

[System.Flags]
public enum DamageFlags
{
    None       = 0,
    Invincible = 1 << 0,
    Riding     = 1 << 1
}

public struct DamageContext
{
    public GameObject Dealer;
    public GameObject Target;
    public int Amount;
    public DamageFlags Flags;

    public DamageContext(GameObject dealer, GameObject target, int amount)
    {
        Dealer = dealer;
        Target = target;
        Amount = amount;
        Flags = DamageFlags.None;

        if (target.TryGetComponent<IInvincible>(out var inv) && inv.IsInvincible)
            Flags |= DamageFlags.Invincible;

        if (target.TryGetComponent<RideController>(out var rc) && rc.IsRiding)
            Flags |= DamageFlags.Riding;
    }
}
