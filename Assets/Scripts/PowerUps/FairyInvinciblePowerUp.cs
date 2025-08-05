using UnityEngine;
using System;
using System.Collections;

public class FairyInvinciblePowerUp : MonoBehaviour, IInvincible
{
    private bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;

    [SerializeField] private float powerUpDuration = 10f;

    public void ActivateInvincibility(Action onEnd = null)
    {
        StartCoroutine(ActivateInvincibilityTimer(onEnd));
    }

    private IEnumerator ActivateInvincibilityTimer(Action onEnd)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(powerUpDuration);
        _isInvincible = false;

        onEnd?.Invoke();
    }
}
