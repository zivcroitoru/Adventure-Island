using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public abstract class PickUp : MonoBehaviour, IPickable
{
    private bool _collected;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_collected) return;

        if (!col.CompareTag("Player"))
            return;

        _collected = true;
        Collect(col.gameObject);
    }
    void OnEnable()
    {
        _collected = false;
    }

    public virtual void Collect(GameObject target)
    {
        OnPickUp(target);
        gameObject.SetActive(false);
    }

    protected abstract void OnPickUp(GameObject player);
}
