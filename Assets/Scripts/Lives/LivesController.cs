using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LivesController : MonoBehaviour
{
    [Header("Lives Config")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private ResetPosition resetPosition;

    private int currentLives;

    // Events
    public event Action<int> OnLivesChanged;
    public event Action OnOutOfLives;

    public int CurrentLives => currentLives;

    private void Awake()
    {
        currentLives = startingLives;
        NotifyLivesChanged();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        FruitController.OnBonusLifeEarned += GrantBonusLife;
    }

    private void UnsubscribeFromEvents()
    {
        FruitController.OnBonusLifeEarned -= GrantBonusLife;
    }

    public void LoseLife()
    {
        ModifyLives(-1);

        if (currentLives > 0)
        {
            HandleLifeLost();
        }
        else
        {
            HandleGameOver();
        }
    }

    private void GrantBonusLife()
    {
        ModifyLives(+1);
        Debug.Log("[LivesController] Bonus life granted from fruit collection!");
    }

    private void ModifyLives(int amount)
    {
        currentLives += amount;
        NotifyLivesChanged();
    }

    private void NotifyLivesChanged()
    {
        OnLivesChanged?.Invoke(currentLives);
    }

    private void HandleLifeLost()
    {
        Debug.Log($"[LivesController] Life lost. Remaining: {currentLives}");
        resetPosition?.ResetPlayerPosition();
    }

    private void HandleGameOver()
    {
        Debug.Log("[LivesController] Out of lives. Reloading scene...");
        OnOutOfLives?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
