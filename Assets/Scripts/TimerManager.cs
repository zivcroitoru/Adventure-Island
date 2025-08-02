using UnityEngine;
using TMPro;
using System.Collections;


public class TimerManager : MonoBehaviour
{
    public int startTime = 10;
    private float timer;

    public TextMeshProUGUI timerText;
    public GameObject player;

    void Start()
    {
        timer = startTime;

        if (timerText == null)
            timerText = FindObjectOfType<TextMeshProUGUI>();
    }

    void Update()
    {
        // Toggle full game speedup with F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 5f : 1f;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            int displayTime = Mathf.CeilToInt(timer);
            timerText.text = $"{displayTime}<size=60%> SEC</size>";
        }
        else
        {
            KillPlayer();
        }
    }

  void KillPlayer()
{
    if (player != null)
    {
        Debug.Log("Time's up! Player is dead.");
        SC_Death.TriggerSpikeCollision(); // Play animation
        StartCoroutine(WaitBeforeDisable()); // Start delay
    }
}

private IEnumerator WaitBeforeDisable()
{
    yield return new WaitForSeconds(5f); // Wait for animation to play
    enabled = false;
    
    // Optional: Destroy player after animation
    // Destroy(player);
}
}
