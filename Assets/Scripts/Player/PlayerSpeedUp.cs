using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedUp : MonoBehaviour
{
    private bool _isSpeedUp = false;

    // Public getter so other scripts can check if we're sped up
    public bool IsSpeedUp { get => _isSpeedUp; }

    // How long the speed boost lasts
    public float powerUpDuration = 5f;

    private SpriteRenderer _curSpriteRenderer;

    private void Awake()
    {
        // Grab the SpriteRenderer so we can change color when boosted
        _curSpriteRenderer = GetComponent<SpriteRenderer>();    
    }
    
    public void ActivateSpeedUp()
    {
        // Start the speed-up effect (called from outside)
        Debug.Log("ActivateSpeedUp");
        StartCoroutine(ActivateSpeedUpTimer());
    }

    IEnumerator ActivateSpeedUpTimer()
    {
        _isSpeedUp = true;

        // Change color to blue so player knows something's up
        _curSpriteRenderer.color = Color.blue;

        // Try to boost movement speed
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.speed *= 1.5f; // make the player faster
        }

        // Wait until power-up ends
        yield return new WaitForSeconds(powerUpDuration);

        // Revert speed back to normal
        if (movement != null)
        {
            movement.speed /= 1.5f;
        }

        // Change color back to normal and reset state
        _curSpriteRenderer.color = Color.white;
        _isSpeedUp = false;
    }
}
