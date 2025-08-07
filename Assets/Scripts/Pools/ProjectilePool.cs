using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool<T> : MonoBehaviour where T : BaseProjectile
{
    [Header("Pool Settings")]
    [SerializeField] private T prefab;
    [SerializeField] private int defaultCapacity = 5;
    [SerializeField] private int maxSize         = 10;

    private ObjectPool<T> pool;

    private void Awake()
    {
        pool = new ObjectPool<T>(
            CreateItem,
            OnGetItem,
            OnReleaseItem,
            OnDestroyItem,
            collectionCheck: false,
            defaultCapacity,
            maxSize);
    }

    /* ───── Public API ───── */
    public T Get(Vector3 position, Quaternion rotation)
    {
        var item = pool.Get();
        item.transform.SetPositionAndRotation(position, rotation);
        return item;
    }

    public void Release(T item)
    {
        pool.Release(item);
    }

    /* ───── Pool Callbacks ───── */
    private T CreateItem()
    {
        var instance = Instantiate(prefab, transform);
        instance.SetPool(this);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGetItem(T item)
    {
        if (item == null)
        {
            Debug.LogError("[ProjectilePool] ❌ Null projectile fetched from pool.");
            return;
        }

        item.gameObject.SetActive(true);
        item.OnSpawn();
    }

    private void OnReleaseItem(T item)
    {
        item.OnDespawn();
        item.gameObject.SetActive(false);
    }

    private void OnDestroyItem(T item)
    {
        Debug.LogWarning($"[ProjectilePool] ⚠️ Attempted to destroy pooled item {item.name}. Ignored.");
        // Don't actually destroy pooled objects.
    }
}
