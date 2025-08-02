using UnityEngine;
using UnityEngine.UI;

public class EnergyView : MonoBehaviour
{
    [SerializeField] private Image energyBarFill; // One Image using fillAmount

    public void UpdateDisplay(float currentEnergy, float maxEnergy)
    {
        if (energyBarFill == null || maxEnergy <= 0f) return;

        // Calculate raw fill ratio
        float rawFill = currentEnergy / maxEnergy;

        // Snap to 3-decimal precision (e.g., 0.733)
        float fillAmount = Mathf.Floor(rawFill * 1000f) / 1000f;

        // Apply to UI
        energyBarFill.fillAmount = fillAmount;

        // Debug logs
        Debug.Log($"[EnergyView] Energy: {currentEnergy:0.000}/{maxEnergy} → FillAmount: {fillAmount:0.000}");
    }
}
