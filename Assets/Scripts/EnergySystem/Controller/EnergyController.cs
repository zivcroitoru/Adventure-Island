using UnityEngine;
using System;
using System.Collections;

[DisallowMultipleComponent]
public sealed class EnergyController : MonoBehaviour, IDamageable, IResettable
{
    [Header("Config")]
    [SerializeField] private int totalBars = 15;
    [SerializeField] private float secondsPerBarLoss = 3f;

    [Header("References")]
    [SerializeField] private EnergyView energyView;
    [SerializeField] private LivesController livesManager;

    private IEnergyModel model;
    private EnergyDecay decay;

    public bool IsDepleted { get; private set; }

    public event Action<int> OnDamageTaken;

    private void Awake()
    {
        Debug.Log("[EnergyController] Awake → Registering for game reset.");
        GameResetManager.Instance?.Register(this);
    }

    private void Start()
    {
        Debug.Log("[EnergyController] Start → Initializing model and decay.");
        model = new EnergyModel(totalBars);
        decay = new EnergyDecay(model, secondsPerBarLoss, UpdateView, OnEnergyDepleted);
        UpdateView();
        decay.StartDecay(this);
    }

    public void TakeDamage(int amount)
    {
        if (TryGetComponent<RideController>(out var rc) && rc.IsRiding)
        {
            Debug.Log("[EnergyController] Player is riding → Forcing dismount.");
            rc.DismountCurrentAnimal();
            return;
        }

        model.Decrease(amount);
        Debug.Log($"[EnergyController] Took {amount} damage → Energy: {model.CurrentEnergy}/{model.MaxEnergy}");

        UpdateView();
        OnDamageTaken?.Invoke(amount);

        if (model.CurrentEnergy <= 0)
        {
            Debug.Log("[EnergyController] Energy depleted from damage → Triggering death.");
            TriggerEnergyDeath();
        }
    }

    public void AddBars(int bars)
    {
        model.Add(bars);
        Debug.Log($"[EnergyController] Gained {bars} energy → Energy: {model.CurrentEnergy}/{model.MaxEnergy}");
        UpdateView();
    }

    public void ResetEnergy(bool restartDecay = true)
    {
        Debug.Log("[EnergyController] ResetEnergy called.");
        model.Reset();
        IsDepleted = false;
        UpdateView();

        if (restartDecay)
            decay.StartDecay(this);
    }

    public void ResetState() => ResetEnergy();

    private void OnEnergyDepleted()
    {
        Debug.Log("[EnergyController] Energy fully depleted from decay.");
        TriggerEnergyDeath();
    }

    private void TriggerEnergyDeath()
    {
        if (IsDepleted) return;

        Debug.Log("[EnergyController] Triggering energy death → Pausing game.");
        IsDepleted = true;
        Time.timeScale = 0f;
        StartCoroutine(UnfreezeAndHandleDeath());
    }

    private IEnumerator UnfreezeAndHandleDeath()
    {
        yield return new WaitForSecondsRealtime(1f);
        // Debug.Log("[EnergyController] Resuming game → Losing life.");
        Time.timeScale = 1f;
        livesManager?.LoseLife();
        ResetEnergy();
    }

    private void UpdateView()
    {
        // Debug.Log($"[EnergyController] Updating UI → Energy: {model.CurrentEnergy}/{totalBars}");
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
    }
}
