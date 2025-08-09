using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class FairyInvinciblePowerUp : MonoBehaviour, IInvincible
{
    [Header("Durations (sec)")]
    [SerializeField] float powerUpDuration     = 10f;
    [SerializeField] float defaultTempDuration = 1.5f;

    public bool IsInvincible    => _powerUpActive || _tempActive;
    public bool IsPowerUpActive => _powerUpActive;
    public bool IsTempActive    => _tempActive;

    public event Action PowerUpStarted;
    public event Action PowerUpEnded;
    public event Action TempStarted;
    public event Action TempEnded;

    bool _powerUpActive, _tempActive;
    Coroutine _powerUpCo, _tempCo;

    // === API ===

    public void ActivateInvincibility(float? duration = null, Action onEnd = null)
    {
        float dur = Mathf.Max(0f, duration ?? powerUpDuration);
        if (_powerUpCo != null) StopCoroutine(_powerUpCo);
        _powerUpCo = StartCoroutine(PowerUpTimer(dur, onEnd));
    }

    public void ActivateTemp(float? duration = null)
    {
        float dur = Mathf.Max(0f, duration ?? defaultTempDuration);
        if (_tempCo != null) StopCoroutine(_tempCo);
        _tempCo = StartCoroutine(TempTimer(dur));
    }

    public void SetTemporaryInvincibility(bool state)
    {
        if (state) ActivateTemp(defaultTempDuration);
        else EndTempImmediate();
    }

    public void ClearAll()
    {
        if (_powerUpCo != null) StopCoroutine(_powerUpCo);
        _powerUpCo = null;
        if (_powerUpActive)
        {
            _powerUpActive = false;
            PowerUpEnded?.Invoke();
        }
        if (_tempCo != null) StopCoroutine(_tempCo);
        _tempCo = null;
        if (_tempActive)
        {
            _tempActive = false;
            TempEnded?.Invoke();
        }
    }

    void OnDisable() => ClearAll();

    // === Coroutines ===

    IEnumerator PowerUpTimer(float seconds, Action onEnd)
    {
        _powerUpActive = true;
        PowerUpStarted?.Invoke();

        if (seconds > 0f)
            yield return new WaitForSeconds(seconds);

        _powerUpActive = false;
        _powerUpCo = null;
        PowerUpEnded?.Invoke();
        onEnd?.Invoke();
    }

    IEnumerator TempTimer(float seconds)
    {
        _tempActive = true;
        TempStarted?.Invoke();

        if (seconds > 0f)
            yield return new WaitForSeconds(seconds);

        EndTempImmediate();
    }

    void EndTempImmediate()
    {
        if (_tempCo != null) StopCoroutine(_tempCo);
        _tempCo = null;

        if (_tempActive)
        {
            _tempActive = false;
            TempEnded?.Invoke();
        }
    }
}
