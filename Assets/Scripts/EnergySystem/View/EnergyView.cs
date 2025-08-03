using UnityEngine;
using UnityEngine.UI;

public class EnergyView : MonoBehaviour
{
    [SerializeField] private Image energyBarFill; // One Image using fillAmount

    public void UpdateDisplay(float currentEnergy, float maxEnergy)
    {
        if (energyBarFill == null || maxEnergy <= 0f) return;

        float rawFill = currentEnergy / maxEnergy;
        float fillAmount = Mathf.Floor(rawFill * 1000f) / 1000f;
        energyBarFill.fillAmount = fillAmount;
    }
}
