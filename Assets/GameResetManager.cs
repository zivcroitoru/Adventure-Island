using System.Collections.Generic;
using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance;

    private readonly List<IResettable> resettables = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[GameResetManager] Singleton instance created.");
        }
        else
        {
            Debug.LogWarning("[GameResetManager] Duplicate instance destroyed.");
            Destroy(gameObject);
        }
    }

public void Register(IResettable resettable)
{
    Debug.Log($"[GameResetManager] Register() called for {resettable.GetType().Name} ({((MonoBehaviour)resettable).gameObject.name})");
    if (!resettables.Contains(resettable))
    {
        resettables.Add(resettable);
        Debug.Log($"[GameResetManager] Registered: {resettable.GetType().Name} ({((MonoBehaviour)resettable).gameObject.name})");
    }
}

    public void ResetAll()
    {
        Debug.Log($"[GameResetManager] Resetting {resettables.Count} registered objects.");
        foreach (var r in resettables)
        {
            Debug.Log($"[GameResetManager] Resetting: {r.GetType().Name} ({r})");
            r.ResetState();
        }
    }
}
