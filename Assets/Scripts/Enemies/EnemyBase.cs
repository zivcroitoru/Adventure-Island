using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Egg & Respawn")]
    [SerializeField] private EggPickup eggPrefab;
    [SerializeField] private float respawnDelay = 8f;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    private void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;
    }

public virtual void TakeDamage(int amount)
{
    Debug.Log("[EnemyBase] Took damage → dropping egg & disabling.");

    if (eggPrefab != null)
        Instantiate(eggPrefab, transform.position, Quaternion.identity);

    // ✅ Start coroutine before disabling self
    StartCoroutine(RespawnRoutine());

    // ❌ Disable after coroutine is scheduled
    gameObject.SetActive(false);
}


    private System.Collections.IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        transform.SetPositionAndRotation(_spawnPos, _spawnRot);
        gameObject.SetActive(true);
    }
}
