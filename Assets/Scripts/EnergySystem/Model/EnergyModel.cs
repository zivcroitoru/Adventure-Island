using UnityEngine;

public sealed class EnergyModel : IEnergyModel, IResettable
{
    public float CurrentEnergy { get; private set; }
    public float MaxEnergy     { get; private set; }

    public EnergyModel(float maxEnergy)
    {
        MaxEnergy     = maxEnergy;
        CurrentEnergy = maxEnergy;
    }

    public void Add(float amount)        => CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy);
    public void Decrease(float delta)    => CurrentEnergy = Mathf.Max(CurrentEnergy - delta, 0f);
    public void Reset()                  => CurrentEnergy = MaxEnergy;
    public void Set(float value)         => CurrentEnergy = Mathf.Clamp(value, 0f, MaxEnergy);

    // IResettable
    public void ResetState() => Reset();
}
