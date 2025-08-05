using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public class ObjectPool<T> : MonoBehaviour where T : Component, IPoolable
{
    [SerializeField] private T prefab;
    [SerializeField] private int initialSize = 16;
    [SerializeField] private bool expandable = true;

    private readonly Queue<T> _items = new();
    private IObjectResolver _resolver;

    [Inject]
    private void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
        PrewarmPool();
    }

    public T Spawn(Vector3 pos, Quaternion rot)
    {
        if (_items.Count == 0 && expandable)
            _items.Enqueue(Create());

        T item = _items.Dequeue();
        item.transform.SetPositionAndRotation(pos, rot);
        item.gameObject.SetActive(true);
        item.OnSpawn();
        return item;
    }

    public void Despawn(T item)
    {
        item.ResetState();
        item.gameObject.SetActive(false);
        _items.Enqueue(item);
    }

    /* ──────────── Helpers ──────────── */
    private void PrewarmPool()
    {
        for (int i = 0; i < initialSize; i++)
            _items.Enqueue(Create());
    }

    private T Create()
    {
        T instance = _resolver.Instantiate(prefab, transform);
        instance.gameObject.SetActive(false);
        return instance;
    }
}
