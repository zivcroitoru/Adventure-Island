using UnityEngine;

// This class stores and manages the player's energy level
public class EnergyModel : IEnergyModel
{
    // How much energy the player currently has
    public float CurrentEnergy { get; private set; }

    // The maximum energy the player can have
    public float MaxEnergy { get; private set; }

    // When we create this, we set both current and max energy
    public EnergyModel(float maxEnergy)
    {
        MaxEnergy = maxEnergy;
        CurrentEnergy = maxEnergy;
    }

    // Adds energy, but never goes over the max
    public void Add(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy);
    }

    // Takes away energy, but never drops below zero
    public void Decrease(float delta)
    {
        CurrentEnergy = Mathf.Max(CurrentEnergy - delta, 0);
    }

    // Fully refills the player's energy
    public void Reset()
    {
        CurrentEnergy = MaxEnergy;
    }

    // Sets energy to a specific value, safely clamped between 0 and max
    public void Set(float value)
    {
        CurrentEnergy = Mathf.Clamp(value, 0, MaxEnergy);
    }
}
