using System;
using System.Collections;
using UnityEngine;

// Handles passive energy decay over time
public class EnergyDecay
{
    private readonly IEnergyModel model;
    private readonly float interval;
    private readonly Action onChanged;
    private readonly Action onDepleted;

    private Coroutine decayRoutine;

    public EnergyDecay(IEnergyModel model, float interval, Action onChanged, Action onDepleted)
    {
        this.model = model;
        this.interval = interval;
        this.onChanged = onChanged;
        this.onDepleted = onDepleted;
    }

    public void StartDecay(MonoBehaviour context)
    {
        Stop(context);
        Debug.Log("[EnergyDecay] Starting decay coroutine.");
        decayRoutine = context.StartCoroutine(DecayLoop());
    }

    private IEnumerator DecayLoop()
    {
        Debug.Log("[EnergyDecay] Decay loop started.");
        while (model.CurrentEnergy > 0)
        {
            yield return new WaitForSeconds(interval);

            model.Decrease(1);
            Debug.Log($"[EnergyDecay] Energy decreased. Current: {model.CurrentEnergy}");

            onChanged?.Invoke();

            if (model.CurrentEnergy <= 0)
                break;
        }

        decayRoutine = null;
        Debug.Log("[EnergyDecay] Energy depleted. Triggering onDepleted.");
        onDepleted?.Invoke();
    }

    public void Stop(MonoBehaviour context)
    {
        if (decayRoutine != null)
        {
            context.StopCoroutine(decayRoutine);
            decayRoutine = null;
        }
    }
}
