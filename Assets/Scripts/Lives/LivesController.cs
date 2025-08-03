using UnityEngine;

public class LivesController : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private ResetPosition resetPosition; // Drag the player's ResetPosition script here

    private int currentLives;

    public System.Action<int> OnLivesChanged;
    public System.Action OnOutOfLives;

    private void Awake()
    {
        currentLives = startingLives;
        OnLivesChanged?.Invoke(currentLives);
    }

    public void LoseLife()
    {
        currentLives--;

        OnLivesChanged?.Invoke(currentLives);

        if (currentLives > 0)
        {
            resetPosition?.ResetPlayerPosition(); // 👈 reset only if still alive
        }
        else
        {
            OnOutOfLives?.Invoke(); // 👈 game over
        }
    }

    public int GetCurrentLives() => currentLives;
}
