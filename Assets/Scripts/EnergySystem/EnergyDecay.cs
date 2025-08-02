using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDecay
{
    private readonly IEnergyModel model;
    private readonly float interval;
    private readonly System.Action onChanged;
    private readonly System.Action onDepleted;

    private Coroutine decayRoutine;

    public EnergyDecay(IEnergyModel model, float interval, System.Action onChanged, System.Action onDepleted)
    {
        this.model = model;
        this.interval = interval;
        this.onChanged = onChanged;
        this.onDepleted = onDepleted;
    }

    public void StartDecay(MonoBehaviour context)
    {
        Stop(context);
        decayRoutine = context.StartCoroutine(DecayLoop());
    }

    public void ResumeIfNeeded(MonoBehaviour context)
    {
        if (decayRoutine == null && model.CurrentEnergy > 0)
            StartDecay(context);
    }

    private IEnumerator DecayLoop()
    {
        while (model.CurrentEnergy > 0)
        {
            yield return new WaitForSeconds(interval);
            model.Decrease(1);
            onChanged?.Invoke();
        }

        decayRoutine = null;
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
