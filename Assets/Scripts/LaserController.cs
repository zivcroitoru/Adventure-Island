using UnityEngine;

// Handles what happens when the player picks up a laser power-up
public class LaserController : PickUp
{
    // Called when the player touches the pickup
    protected override void OnPickUp(GameObject player)
    {
        // Give the player the laser power-up
        player.GetComponent<PlayerPowerUp>().CollectPowerUp(new LaserPowerUp());
    }
}
