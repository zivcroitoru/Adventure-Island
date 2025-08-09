using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool<T> : MonoBehaviour where T : BaseProjectile
{
    [Header("Pool Settings")]
    [SerializeField] private T prefab;
    [SerializeField] private int defaultCapacity = 5;
    [SerializeField] private int maxSize         = 10;
    [SerializeField] private Transform container; // optional parent for spawned items

    private ObjectPool<T> pool;

    private void Awake()
    {
        // In dev, consider collectionCheck:true to catch double-release bugs early
        pool = new ObjectPool<T>(
            CreateItem,
            OnGetItem,
            OnReleaseItem,
            OnDestroyItem,
            collectionCheck: false,
            defaultCapacity,
            maxSize
        );
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
        // One-way: never calls item.ReturnToPool() here (avoids recursion)
        pool.Release(item);
    }

    /* ───── Pool Callbacks ───── */
    private T CreateItem()
    {
        var parent = container ? container : transform;
        var instance = Instantiate(prefab, parent);

        // Wire the projectile to this pool (uses the overload you added)
        instance.SetPool(this);

        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGetItem(T item)
    {
        if (!item)
        {
            Debug.LogError("[ProjectilePool] ❌ Null projectile fetched.");
            return;
        }

        item.gameObject.SetActive(true);
        item.OnSpawn(); // reset runtime state; no Release calls inside
    }

    private void OnReleaseItem(T item)
    {
        // Reset-only. Do not call ReturnToPool() or invoke gameplay events here.
        item.OnDespawn();
        item.gameObject.SetActive(false);
    }

    private void OnDestroyItem(T item)
    {
        // This is only called if the pool decides to actually destroy an instance
        if (item)
            Destroy(item.gameObject);
    }
}
