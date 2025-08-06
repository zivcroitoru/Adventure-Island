using TMPro;
using UnityEngine;

public class FruitViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitText;

private void OnEnable()
{
    FruitController.OnFruitCountChanged += UpdateUI;
    UpdateUI(FruitController.TotalFruitsCollected); // sync immediately
}


    private void OnDisable()
    {
        FruitController.OnFruitCountChanged -= UpdateUI;
    }

    private void UpdateUI(int fruitCount)
    {
        Debug.Log($"[FruitViewUI] Updating Fruit Count: {fruitCount}");
        fruitText.text = $"x {fruitCount}";
    }
}
