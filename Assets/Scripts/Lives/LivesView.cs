using UnityEngine;
using TMPro;

public class LivesView : MonoBehaviour
{
    [SerializeField] private LivesController livesController;
    [SerializeField] private TextMeshProUGUI livesText;

private void OnEnable()
{
    livesController.OnLivesChanged += UpdateUI;
    UpdateUI(livesController.CurrentLives); // show lives immediately
}

private void OnDisable()
{
    livesController.OnLivesChanged -= UpdateUI; // cleanup
}


    private void UpdateUI(int lives)
    {
        livesText.text = $"x {lives}";
    }
}
