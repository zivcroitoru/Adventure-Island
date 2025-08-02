using UnityEngine;
using VContainer;

public class PlayerDamageController : MonoBehaviour, IDamageable
{
    private IEnergyModel _energy;

    [Inject]
    public void Construct(IEnergyModel energyModel)
    {
        _energy = energyModel;
    }

    public void TakeDamage(int amount)
    {
        _energy.Decrease(amount);
    }
}
