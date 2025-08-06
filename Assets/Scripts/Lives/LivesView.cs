using UnityEngine;
using TMPro;

public class LivesView : MonoBehaviour
{
    [SerializeField] private LivesController livesController;
    [SerializeField] private TextMeshProUGUI livesText;

    private void OnEnable()
    {
        livesController.OnLivesChanged += UpdateUI;
    }

    private void OnDisable()
    {
        livesController.OnLivesChanged -= UpdateUI;
    }

    private void UpdateUI(int lives)
    {
        livesText.text = $"x {lives}";
    }
}
