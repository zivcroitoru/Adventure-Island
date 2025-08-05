using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool<T> : MonoBehaviour where T : BaseProjectile
{
    [SerializeField] T prefab;
    [SerializeField] int defaultCapacity = 5;
    [SerializeField] int maxSize        = 10;

    ObjectPool<T> pool;

    void Awake()
    {
        pool = new ObjectPool<T>(
            Create, OnGet, OnRelease, OnDestroyPoolItem,
            collectionCheck: false,
            defaultCapacity, maxSize);
    }

    /* ───── Public API ───── */
    public T Get(Vector3 pos, Quaternion rot)
    {
        var p = pool.Get();
        p.transform.SetPositionAndRotation(pos, rot);
        return p;
    }

    public void Release(T item) => pool.Release(item);

    /* ───── Callbacks ───── */
    T Create()
    {
        var p = Instantiate(prefab, transform);
        p.SetPool(this);
        p.gameObject.SetActive(false);
        return p;
    }

    void OnGet   (T p){ p.gameObject.SetActive(true);  p.OnSpawn();   }
    void OnRelease(T p){ p.OnDespawn(); p.gameObject.SetActive(false); }
    void OnDestroyPoolItem(T p)=> Destroy(p.gameObject);
}
