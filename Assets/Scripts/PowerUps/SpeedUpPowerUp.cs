using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            Debug.Log("Start Speed Up!");
            // Try to get the PlayerSpeedUp component from the player
            PlayerSpeedUp playerSpeedUp = player.GetComponent<PlayerSpeedUp>();
            if (playerSpeedUp != null)
                playerSpeedUp.ActivateSpeedUp();
        }
    }
}
