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
    }

    public void Register(EnemyBase enemy)
    {
        enemy.OnDeath += HandleDeath;
    }

    private void HandleDeath(EnemyBase enemy, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(RespawnAfterDelay(enemy, pos, rot, defaultRespawnDelay));
    }

private IEnumerator RespawnAfterDelay(EnemyBase enemy, Vector3 pos, Quaternion rot, float delay)
{
    yield return new WaitForSeconds(delay);

    enemy.transform.SetPositionAndRotation(pos, rot);
    enemy.PrepareForRespawn();       // <-- make it alive & with HP
    enemy.gameObject.SetActive(true);
}

    public void RequestRespawn(EnemyBase enemy, Vector3 pos, Quaternion rot)
{
    StartCoroutine(RespawnAfterDelay(enemy, pos, rot, defaultRespawnDelay));
}

}
