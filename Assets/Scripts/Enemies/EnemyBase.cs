using UnityEngine;

[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public event System.Action<EnemyBase, Vector3, Quaternion> OnDeath;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;

    protected virtual void Awake()
    {
        _spawnPos = transform.position;
        _spawnRot = transform.rotation;
    }

    public virtual void TakeDamage(int amount)
    {
        Debug.Log("[EnemyBase] Took damage → broadcasting death.");
        gameObject.SetActive(false);
        OnDeath?.Invoke(this, _spawnPos, _spawnRot);
    }
}
