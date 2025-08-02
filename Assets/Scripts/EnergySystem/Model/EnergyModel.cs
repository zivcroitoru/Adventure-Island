using UnityEngine;

public class EnergyModel : IEnergyModel
{
    public float CurrentEnergy { get; private set; }
    public float MaxEnergy { get; private set; }

    public EnergyModel(float startEnergy)
    {
        MaxEnergy = startEnergy;
        CurrentEnergy = startEnergy;
        Debug.Log($"[EnergyModel] Initialized with {CurrentEnergy}/{MaxEnergy}");
    }

    public void Add(float amount)
    {
        float before = CurrentEnergy;
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy);
        Debug.Log($"[EnergyModel] Add({amount}) → {before} → {CurrentEnergy}");
    }

    public void Decrease(float delta)
    {
        float before = CurrentEnergy;
        CurrentEnergy = Mathf.Max(CurrentEnergy - delta, 0f);
        Debug.Log($"[EnergyModel] Decrease({delta}) → {before} → {CurrentEnergy}");
    }

    public void Reset()
    {
        Debug.Log($"[EnergyModel] Reset from {CurrentEnergy} to {MaxEnergy}");
        CurrentEnergy = MaxEnergy;
    }
}
