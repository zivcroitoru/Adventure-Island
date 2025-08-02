using UnityEngine;
using System.Collections;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private EnergyView energyView;
    [SerializeField] private int totalBars = 15; // Total number of bars (also the max energy value)
    [SerializeField] private float secondsPerBarLoss = 3f;

    private IEnergyModel model;
    private Coroutine decayCoroutine;

    void Start()
    {
        if (model == null)
        {
            model = new EnergyModel(totalBars); // model.MaxEnergy = 15
        }

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

        decayCoroutine = StartCoroutine(EnergyDecayLoop());
    }

    public void Initialize(IEnergyModel model)
    {
        this.model = model;

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

        if (decayCoroutine != null)
            StopCoroutine(decayCoroutine);

        decayCoroutine = StartCoroutine(EnergyDecayLoop());
    }

    private IEnumerator EnergyDecayLoop()
    {
        while (model.CurrentEnergy > 0)
        {
            model.Decrease(1); // Lose 1 bar
            energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

            yield return new WaitForSeconds(secondsPerBarLoss);
        }

        decayCoroutine = null;
        enabled = false;
    }

    public void AddBars(int bars)
    {
        float newEnergy = Mathf.Min(model.CurrentEnergy + bars, totalBars);
        model.Set(newEnergy);

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

        if (decayCoroutine == null && model.CurrentEnergy > 0)
            decayCoroutine = StartCoroutine(EnergyDecayLoop());
    }

    public void RemoveBars(int bars)
    {
        float newEnergy = Mathf.Max(model.CurrentEnergy - bars, 0);
        model.Set(newEnergy);

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
    }

    public void ResetEnergy()
    {
        model.Reset();
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

        if (decayCoroutine == null)
            decayCoroutine = StartCoroutine(EnergyDecayLoop());
    }

    public int GetCurrentBars()
    {
        return Mathf.FloorToInt(model.CurrentEnergy);
    }

    public int GetMaxBars()
    {
        return totalBars;
    }

    public bool IsEmpty()
    {
        return model.CurrentEnergy <= 0f;
    }
}
