using UnityEngine;

public class EnergyModel : IEnergyModel
{
    public float CurrentEnergy { get; private set; }
    public float MaxEnergy { get; private set; }

    public EnergyModel(float maxEnergy)
    {
        MaxEnergy = maxEnergy;
        CurrentEnergy = maxEnergy;
    }

    public void Add(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy);
    }

    public void Decrease(float delta)
    {
        CurrentEnergy = Mathf.Max(CurrentEnergy - delta, 0);
    }

    public void Reset()
    {
        CurrentEnergy = MaxEnergy;
    }

    public void Set(float value)
    {
        CurrentEnergy = Mathf.Clamp(value, 0, MaxEnergy);
    }
}

