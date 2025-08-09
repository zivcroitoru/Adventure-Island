using UnityEngine;
using System;
using System.Collections;

[DisallowMultipleComponent]
public sealed class EnergyController : MonoBehaviour, IDamageable, IResettable
{
    [Header("Config")]
    [SerializeField] private int totalBars = 15;
    [SerializeField] private float secondsPerBarLoss = 3f;
    [SerializeField] private float damageCooldown = 0.5f; // min seconds between hits

    [Header("References")]
    [SerializeField] private EnergyView energyView;
    [SerializeField] private LivesController livesManager;

    private IEnergyModel model;
    private EnergyDecay decay;

    private float _lastDamageTime = -999f; // track last time damage was applied

    public bool IsDepleted { get; private set; }
    public event Action<int> OnDamageTaken;

    private void Awake()
    {
        if (model == null) model = new EnergyModel(totalBars);
        if (energyView == null) energyView = FindObjectOfType<EnergyView>(true);

        GameResetManager.Instance?.Register(this);
        UpdateView();
    }

    private void Start()
    {
        if (decay == null)
            decay = new EnergyDecay(model, secondsPerBarLoss, UpdateView, OnEnergyDepleted);

        UpdateView();
        decay.StartDecay(this);
    }

    public void TakeDamage(int amount, GameObject dealer)
    {
        Debug.Log($"[Energy] TakeDamage dealer={dealer?.name}, amount={amount}");
        TakeDamage(amount);
    }

    public void TakeDamage(int amount)
    {
        // === cooldown check ===
        if (Time.time - _lastDamageTime < damageCooldown) return;
        _lastDamageTime = Time.time;

        if (amount <= 0 || IsDepleted) return;

        int before = (int)model.CurrentEnergy;
        model.Add(-amount);
        int after = Mathf.Max(0, (int)model.CurrentEnergy);

        OnDamageTaken?.Invoke(amount);
        UpdateView();

        if (after <= 0)
        {
            if (TryGetComponent<RideController>(out var rc) && rc.IsRiding)
            {
                rc.DismountCurrentAnimal();
                AddBars(1);
                IsDepleted = false;
                return;
            }

            TriggerEnergyDeath();
            return;
        }
    }

    public void AddBars(int bars)
    {
        model.Add(bars);
        // Debug.Log($"[Energy] +{bars} → {(int)model.CurrentEnergy}/{totalBars}");
        UpdateView();
    }

    public void ResetEnergy(bool restartDecay = true)
    {
        model.Reset();
        IsDepleted = false;
        UpdateView();
        if (restartDecay) decay.StartDecay(this);
    }

    public void ResetState() => ResetEnergy();

    private void OnEnergyDepleted() => TriggerEnergyDeath();

    private void TriggerEnergyDeath()
    {
        if (IsDepleted) return;
        IsDepleted = true;

        Time.timeScale = 0f;
        StartCoroutine(UnfreezeAndHandleDeath());
    }

    private IEnumerator UnfreezeAndHandleDeath()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        livesManager?.LoseLife();
        ResetEnergy();
    }

    private void UpdateView()
    {
        if (energyView == null) return;
        energyView.UpdateDisplay((int)model.CurrentEnergy, totalBars);
    }
}
