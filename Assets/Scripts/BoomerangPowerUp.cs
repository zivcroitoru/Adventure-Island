using UnityEngine;

public class BoomerangPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        var handler = player.GetComponentInChildren<WeaponsHandler>();
        var boomerang = player.GetComponentInChildren<BoomerangWeapon>();
        handler?.Equip(boomerang);
    }
}
