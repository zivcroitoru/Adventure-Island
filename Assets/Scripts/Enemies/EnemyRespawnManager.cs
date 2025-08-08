using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class EnemyRespawnManager : MonoBehaviour
{
    public static EnemyRespawnManager Instance { get; private set; }

    [SerializeField] private float defaultRespawnDelay = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Debug.Log("[EnemyRespawnManager] ✅ Initialized.");
    }

    public void Register(EnemyBase enemy)
    {
        enemy.OnDeath += HandleDeath;
        // Debug.Log($"[EnemyRespawnManager] 📦 Registered '{enemy.name}'");
    }

    private void HandleDeath(EnemyBase enemy, Vector3 pos, Quaternion rot)
    {
        // Debug.Log($"[EnemyRespawnManager] 💀 '{enemy.name}' died → scheduling respawn.");
        StartCoroutine(RespawnAfterDelay(enemy, pos, rot, defaultRespawnDelay));
    }

    private IEnumerator RespawnAfterDelay(EnemyBase enemy, Vector3 pos, Quaternion rot, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.transform.SetPositionAndRotation(pos, rot);
        enemy.gameObject.SetActive(true);
        // Debug.Log($"[EnemyRespawnManager] ♻️ Respawned '{enemy.name}' at {pos}.");
    }
    public void RequestRespawn(EnemyBase enemy, Vector3 pos, Quaternion rot)
{
    StartCoroutine(RespawnAfterDelay(enemy, pos, rot, defaultRespawnDelay));
}

}
