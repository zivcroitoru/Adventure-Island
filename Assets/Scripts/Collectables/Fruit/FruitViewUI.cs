using TMPro;
using UnityEngine;

public class FruitViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitText;

    private void OnEnable()
    {
        FruitEffect.OnFruitCountChanged += UpdateUI;
        UpdateUI(FruitEffect.TotalFruitsCollected);
    }

    private void OnDisable()
    {
        FruitEffect.OnFruitCountChanged -= UpdateUI;
    }
    private void UpdateUI(int fruitCount)
    {
        // Debug.Log($"[FruitViewUI] Updating Fruit Count: {fruitCount}");
        fruitText.text = $"x {fruitCount}";
    }
}
