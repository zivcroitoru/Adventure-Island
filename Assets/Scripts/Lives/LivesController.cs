using System;
using System.Collections;
using UnityEngine;

public class LivesController : MonoBehaviour, IResettable
{
    [Header("Lives")]
    [SerializeField] private int startingLives = 3;

    [Header("Respawn")]
    [SerializeField] private ResetPosition resetPosition;      // drag your spawn/reset helper
    [SerializeField] private float respawnGraceSeconds = 0.25f; // avoid instant re-hit loops

    private int currentLives;
    private bool _respawning;

    public int CurrentLives => currentLives;
    public event Action<int> OnLivesChanged;
    public event Action OnOutOfLives;

    void Awake() => GameResetManager.Instance?.Register(this);

    void Start()
    {
        if (currentLives <= 0) ResetState(); // ensure proper init in fresh scene
    }

    public void LoseLife()
    {
        if (_respawning || currentLives <= 0) return;

        ModifyLives(-1);

        if (currentLives > 0)
        {
            // Reset only the “life-loss” group (eggs, pickups…)
            GameResetManager.Instance?.ResetOnLifeLost();

            // Reposition player and grant brief grace
            resetPosition?.ResetPlayerPosition();
            StartCoroutine(RespawnIFrames());
        }
        else
        {
            HandleGameOver();
        }
    }

    public void AddLife(int amount = 1) => ModifyLives(+Mathf.Abs(amount));

    public void ResetState() // full reset (called on game over)
    {
        currentLives = Mathf.Max(0, startingLives); // back to 3 (or inspector value)
        NotifyLivesChanged();
        resetPosition?.ResetPlayerPosition();
        _respawning = false;
    }

    private IEnumerator RespawnIFrames()
    {
        _respawning = true;
        yield return new WaitForSeconds(respawnGraceSeconds);
        _respawning = false;
    }

    private void ModifyLives(int delta)
    {
        int before = currentLives;
        currentLives = Mathf.Clamp(currentLives + delta, 0, 99);
        if (currentLives != before) NotifyLivesChanged();
    }

    private void NotifyLivesChanged() => OnLivesChanged?.Invoke(currentLives);

    private void HandleGameOver()
    {
        OnOutOfLives?.Invoke();
        GameResetManager.Instance?.ResetAll(); // this will call ResetState() on everyone incl. this
    }
}
