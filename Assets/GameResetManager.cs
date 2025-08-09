using System.Collections.Generic;
using UnityEngine;


public sealed class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance { get; private set; }

    // Full reset (game over / level restart)
    private readonly List<IResettable> _resettables = new();
    // Light reset when a life is lost (eggs, pickups, loose projectiles…)
    private readonly List<IResettable> _resettablesOnLifeLost = new();

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // uncomment if needed
    }

    // Register to full reset; set onLifeLostToo=true to also reset on death
    public void Register(IResettable r, bool onLifeLostToo = false)
    {
        if (r == null) return;
        if (!_resettables.Contains(r)) _resettables.Add(r);
        if (onLifeLostToo && !_resettablesOnLifeLost.Contains(r)) _resettablesOnLifeLost.Add(r);
    }

    public void Unregister(IResettable r)
    {
        if (r == null) return;
        _resettables.Remove(r);
        _resettablesOnLifeLost.Remove(r);
    }

    public void ResetAll()
    {
        Cleanup();
        foreach (var r in _resettables) r?.ResetState();
    }

    public void ResetOnLifeLost()
    {
        Cleanup();
        foreach (var r in _resettablesOnLifeLost) r?.ResetState();
    }

    private void Cleanup()
    {
        _resettables.RemoveAll(IsNull);
        _resettablesOnLifeLost.RemoveAll(IsNull);
        static bool IsNull(IResettable r) => r == null || (r is Object o && o == null);
    }
}
