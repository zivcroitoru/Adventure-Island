using UnityEngine;

public sealed class BoomerangPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        var weapon = player.GetComponentInChildren<BoomerangWeapon>();
        var handler = player.GetComponentInChildren<WeaponsHandler>();
        handler?.Equip(weapon);
    }
}
