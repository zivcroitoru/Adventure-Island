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
        GameResetManager.Instance?.Register(this);
    }

    private void Start()
    {
        model = new EnergyModel(totalBars);
        decay = new EnergyDecay(model, secondsPerBarLoss, OnEnergyChanged, null);

        UpdateView();
        decay.StartDecay(this);
    }

    public void TakeDamage(int amount)
    {
        if (TryGetComponent<RideController>(out var rc) && rc.IsRiding)
        {
            rc.DismountCurrentAnimal();
            return;
        }

        model.Decrease(amount);
        Debug.Log($"[EnergyController] Took {amount} damage! Current energy: {model.CurrentEnergy}");

        UpdateView();
        OnDamageTaken?.Invoke(amount);

        if (model.CurrentEnergy <= 0 && !IsDepleted)
        {
            IsDepleted = true;
            Time.timeScale = 0f;
            StartCoroutine(UnfreezeAndHandleDeath());
        }
    }

    private IEnumerator UnfreezeAndHandleDeath()
    {
        const float pauseDuration = 1f;
        yield return new WaitForSecondsRealtime(pauseDuration);

        Time.timeScale = 1f;
        livesManager?.LoseLife();
        ResetEnergy();
    }

    public void AddBars(int bars)
    {
        model.Add(bars);
        UpdateView();
        decay.ResumeIfNeeded(this);
    }

    public void ResetEnergy()
    {
        model.Reset();
        IsDepleted = false;
        UpdateView();
        decay.StartDecay(this);
    }

    public void ResetState()
    {
        model.Reset();
        IsDepleted = false;
        UpdateView();
    }

    private void OnEnergyChanged()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
    }
}
