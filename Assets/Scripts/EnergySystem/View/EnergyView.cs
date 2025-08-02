using UnityEngine;
using UnityEngine.UI;

public class EnergyView : MonoBehaviour
{
    [SerializeField] private Image energyBarFill; // one Image using fillAmount
    private const int totalBars = 15;

    public void UpdateDisplay(float currentEnergy, float maxEnergy)
    {
        if (energyBarFill == null || maxEnergy <= 0f) return;

        // Calculate how many full bars should be shown
        float energyRatio = currentEnergy / maxEnergy;
        int filledBars = Mathf.FloorToInt(energyRatio * totalBars);
        float fillAmount = filledBars / (float)totalBars;

        // Set fill amount
        energyBarFill.fillAmount = fillAmount;

        // Debug logs
        Debug.Log($"[EnergyView] Energy: {currentEnergy}/{maxEnergy} → Bars: {filledBars}/{totalBars}, FillAmount: {fillAmount:0.000}");
    }
}
