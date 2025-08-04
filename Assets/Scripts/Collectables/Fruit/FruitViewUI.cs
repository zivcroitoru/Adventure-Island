using TMPro;
using UnityEngine;

public class FruitViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitText;

    private void OnEnable()
    {
        FruitController.OnFruitCountChanged += UpdateUI;
    }

    private void OnDisable()
    {
        FruitController.OnFruitCountChanged -= UpdateUI;
    }

    private void UpdateUI(int fruitCount)
    {
        fruitText.text = $"Fruits: {fruitCount}";
    }
}
