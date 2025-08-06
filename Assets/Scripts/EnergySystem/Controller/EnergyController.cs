using UnityEngine;

public class EnergyController : MonoBehaviour, IDamageable, IResettable
{
    [Header("Config")]
    [SerializeField] private int totalBars = 15;
    [SerializeField] private float secondsPerBarLoss = 3f;

    [Header("References")]
    [SerializeField] private EnergyView energyView;
    [SerializeField] private LivesController livesManager;

    private IEnergyModel model;
    private EnergyDecay decay;

    public event System.Action<int> OnDamageTaken;

    private void Start()
    {
        model = new EnergyModel(totalBars);
        decay = new EnergyDecay(model, secondsPerBarLoss, OnEnergyChanged, OnEnergyDepleted);

        Debug.Log($"[EnergyController] Start: Energy={model.CurrentEnergy}, Bars={totalBars}");

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.StartDecay(this);

        if (GameResetManager.Instance != null)
        {
            GameResetManager.Instance.Register(this);
            Debug.Log("[EnergyController] Registered with GameResetManager.");
        }
        else
        {
            Debug.LogWarning("[EnergyController] GameResetManager instance not found!");
        }
    }
    private void Awake()
{
    Debug.Log($"[EnergyController] Awake on GameObject: {gameObject.name}");
    if (GameResetManager.Instance != null)
    {
        GameResetManager.Instance.Register(this);
        Debug.Log("[EnergyController] Registered with GameResetManager (Awake).");
    }
    else
    {
        Debug.LogWarning("[EnergyController] GameResetManager instance not found in Awake!");
    }
}


    public void TakeDamage(int amount)
    {
        if (TryGetComponent<RideController>(out var rc) && rc.IsRiding)
        {
            Debug.Log("[EnergyController] Dismounting animal on damage.");
            rc.DismountCurrentAnimal();
            return;
        }

        model.Decrease(amount);
        Debug.Log($"[EnergyController] Took damage: -{amount} (Now: {model.CurrentEnergy})");

        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);

        OnDamageTaken?.Invoke(amount);

        if (model.CurrentEnergy <= 0)
            OnEnergyDepleted();
    }

    public void AddBars(int bars)
    {
        model.Add(bars);
        Debug.Log($"[EnergyController] Added bars: +{bars} (Now: {model.CurrentEnergy})");
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.ResumeIfNeeded(this);
    }

    public void ResetEnergy()
    {
        model.Reset();
        Debug.Log("[EnergyController] Energy reset and decay restarted.");
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
        decay.StartDecay(this);
    }

    private void OnEnergyChanged()
    {
        Debug.Log($"[EnergyController] Energy changed: {model.CurrentEnergy}");
        energyView?.UpdateDisplay(model.CurrentEnergy, totalBars);
    }

    private void OnEnergyDepleted()
    {
        Debug.Log("[EnergyController] Energy depleted.");
        livesManager?.LoseLife();
        ResetEnergy();
    }

    public void ResetState()
    {
        model.Reset();
        Debug.Log($"[EnergyController] ResetState called. Energy={model.CurrentEnergy}");
        energyView?.UpdateDisplay(model.CurrentEnergy, model.MaxEnergy);
    }
}
