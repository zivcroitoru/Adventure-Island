using UnityEngine;

// Gives the player a laser weapon when picked up
public class LaserPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        Debug.Log("ApplyPowerUp Laser");

        if (player != null)
        {
            // Find the laser weapon on the player (or their children)
            LaserWeapon laserWeapon = player.GetComponentInChildren<LaserWeapon>();

            // If found, equip it
            if (laserWeapon != null)
                laserWeapon.Equip();
        }
    }
}
