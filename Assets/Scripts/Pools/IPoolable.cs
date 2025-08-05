// IPoolable.cs
public interface IPoolable
{
    /// Reset all runtime state (velocity, timers, visuals …) before reuse.
    void ResetState();

    /// Called by the pool right after the object is (re)spawned.
    void OnSpawn();
}
