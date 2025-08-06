using UnityEngine;
using VContainer;

public sealed class EnemyRespawnManager : MonoBehaviour
{
    private void Start()
{
    Debug.Log("[EnemyRespawnManager] ✅ Injected and ready");
}

    [SerializeField] private float defaultRespawnDelay = 8f;

    [Inject] private IObjectResolver _resolver;

    public void ScheduleRespawn(EnemyBase enemy, Vector3 pos, Quaternion rot, float delay = -1f)
    {
        StartCoroutine(RespawnRoutine(enemy, pos, rot, delay < 0 ? defaultRespawnDelay : delay));
    }

    private System.Collections.IEnumerator RespawnRoutine(EnemyBase enemy, Vector3 pos, Quaternion rot, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.transform.SetPositionAndRotation(pos, rot);
        enemy.gameObject.SetActive(true);
    }
}
