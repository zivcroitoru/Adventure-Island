using UnityEngine;
using System;
using System.Collections;

[DisallowMultipleComponent]
public sealed class EnergyController : MonoBehaviour, IDamageable, IResettable
{
    [Header("Config")]
    [SerializeField] private int totalBars = 15;           // integer bars in UI
    [SerializeField] private float secondsPerBarLoss = 3f; // decay rate

    [Header("References")]
    [SerializeField] private EnergyView energyView;        // assign in Inspector; auto-falls back in Awake
    [SerializeField] private LivesController livesManager;

    private IEnergyModel model;    // your implementation may be float-based
    private EnergyDecay decay;

    public bool IsDepleted { get; private set; }

    public event Action<int> OnDamageTaken;

    private void Awake()
    {
        // Ensure model/UI exist even if we get hit before Start()
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

    // === IDamageable (entry point for Damage.Deal) ===
    public void TakeDamage(int amount, GameObject dealer)
    {
        Debug.Log($"[Energy] TakeDamage dealer={dealer?.name}, amount={amount}");
        TakeDamage(amount);
    }

    // Core damage handler (UI + life handling)
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDepleted) return;

        int before = (int)model.CurrentEnergy;
        model.Add(-amount); // your model supports negative deltas
        int after = (int)model.CurrentEnergy;

        Debug.Log($"[Energy] {before} -> {after} (max {(int)model.MaxEnergy})");

        OnDamageTaken?.Invoke(amount);

        if (energyView == null)
            Debug.LogWarning("[Energy] energyView is NULL — UI won’t update!");

        UpdateView();

        if (model.CurrentEnergy <= 0f)
            TriggerEnergyDeath();
    }

    // Positive gain (fruits, pickups)
    public void AddBars(int bars)
    {
        model.Add(bars);
        Debug.Log($"[Energy] +{bars} → {(int)model.CurrentEnergy}/{totalBars}");
        UpdateView();
    }

    // Reset API
    public void ResetEnergy(bool restartDecay = true)
    {
        model.Reset();
        IsDepleted = false;
        UpdateView();
        if (restartDecay) decay.StartDecay(this);
    }

    public void ResetState() => ResetEnergy();

    private void OnEnergyDepleted()
    {
        TriggerEnergyDeath();
    }

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
        // Debug.Log($"[EnergyView] UI set to {(int)model.CurrentEnergy}/{totalBars}");
    }
}
