public interface IEnergyModel
{
    float CurrentEnergy { get; }
    float MaxEnergy { get; }

    void Add(float amount);
    void Decrease(float delta);
    void Reset();
    void Set(float value); // ✅ Add this
}
