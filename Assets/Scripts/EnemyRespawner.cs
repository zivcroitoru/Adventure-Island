using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public sealed class EnemyRespawner : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private float respawnDelay = 5f;

    private void Awake()
    {
        if (enemy != null)
            enemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyBase deadEnemy, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(RespawnAfterDelay(deadEnemy, pos, rot));
    }

    private IEnumerator RespawnAfterDelay(EnemyBase enemy, Vector3 pos, Quaternion rot)
    {
        yield return new WaitForSeconds(respawnDelay);
        enemy.transform.SetPositionAndRotation(pos, rot);
        enemy.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnDeath -= HandleEnemyDeath;
    }
}
