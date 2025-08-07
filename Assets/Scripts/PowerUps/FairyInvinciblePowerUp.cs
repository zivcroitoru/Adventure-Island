using UnityEngine;
using System;
using System.Collections;

public class FairyInvinciblePowerUp : MonoBehaviour, IInvincible
{
    private bool _isInvincible = false;
    private bool _isTempInvincible = false;

    public bool IsInvincible => _isInvincible || _isTempInvincible;

    [SerializeField] private float powerUpDuration = 10f;

    public void ActivateInvincibility(Action onEnd = null)
    {
        Debug.Log("[FairyInvinciblePowerUp] Activating invincibility.");
        StartCoroutine(ActivateInvincibilityTimer(onEnd));
    }

    private IEnumerator ActivateInvincibilityTimer(Action onEnd)
    {
        _isInvincible = true;
        Debug.Log("[FairyInvinciblePowerUp] Invincibility activated.");

        yield return new WaitForSeconds(powerUpDuration);

        _isInvincible = false;
        Debug.Log("[FairyInvinciblePowerUp] Invincibility ended.");

        onEnd?.Invoke();
    }

    public void SetTemporaryInvincibility(bool state)
    {
        _isTempInvincible = state;
        Debug.Log($"[FairyInvinciblePowerUp] Temporary invincibility set to: {_isTempInvincible}");
    }

    public bool IsTemporaryOnly => _isTempInvincible && !_isInvincible;

    // Optional: Log the current invincibility state whenever it's checked
    private void Update()
    {
        if (_isInvincible || _isTempInvincible)
        {
            Debug.Log("[FairyInvinciblePowerUp] Current invincibility state: " + IsInvincible);
        }
    }
}
