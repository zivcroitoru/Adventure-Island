using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private EnergyView energyView;      // Reference to the UI view that displays energy
    [SerializeField] private float startEnergy = 45f;    // Starting energy value, adjustable from the Inspector

    private float energyDecayRate;                       // How much energy is lost per second
    private IEnergyModel model;                          // The model holding energy state

    void Start()
    {
        // If no model was injected, initialize a default one
        if (model == null)
        {
            model = new EnergyModel(startEnergy);
            energyDecayRate = startEnergy / 45f; // Energy drains fully in 45 seconds
        }

        // Update the energy UI once at start
        if (energyView != null)
            energyView.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);
    }

    // Optional external initializer to support DI or advanced setup
    public void Initialize(IEnergyModel model)
    {
        this.model = model;
        energyDecayRate = model.MaxEnergy / 45f;

        if (energyView != null)
            energyView.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);
    }

    void Update()
    {
        if (model == null || energyView == null) return;

        // Reduce energy based on time passed
        model.Decrease(energyDecayRate * Time.deltaTime);

        // Update the UI
        energyView.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);

        // Disable this controller if energy is depleted
        if (model.CurrentEnergy <= 0)
        {
            enabled = false;
        }
    }

    // Public method to add energy manually (e.g., via pickups)
    public void AddEnergy(int amount)
    {
        model.Add(amount);
        energyView.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);
    }

    // Public method to reset energy to full
    public void ResetEnergy()
    {
        model.Reset();
        energyView.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);
    }
}
