using UnityEngine;
using System.Collections;

public class EnergyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnergyView energyView;
    [SerializeField] private int totalBars = 15;
    [SerializeField] private float secondsPerBarLoss = 3f;

    private IEnergyModel model;
    private EnergyDecay decay;

    void Start()
    {
        model = new EnergyModel(totalBars);
        decay = new EnergyDecay(model, secondsPerBarLoss, OnEnergyChanged, OnEnergyDepleted);
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.StartDecay(this);
    }

    public void TakeDamage(int amount)
    {
        model.Decrease(amount);
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        Debug.Log($"[EnergyController] Took {amount} damage");
    }

    public void AddBars(int bars)
    {
        model.Add(bars);
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.ResumeIfNeeded(this);
    }

    public void ResetEnergy()
    {
        model.Reset();
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.StartDecay(this);
    }

    private void OnEnergyChanged() =>
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

    private void OnEnergyDepleted() =>
        enabled = false;
}
