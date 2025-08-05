using UnityEngine;

public abstract class PickUp : MonoBehaviour, IPickable
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            Debug.Log($"[PickUp] Ignored collision with '{col.name}' (tag: {col.tag})");
            return;
        }

        Debug.Log($"[PickUp] ✅ Triggered by Player: '{col.name}'");
        Collect(col.gameObject);
    }

    public virtual void Collect(GameObject target)
    {
        Debug.Log($"[PickUp] 🧲 Collect() called for '{target.name}' by pickup '{name}'");
        OnPickUp(target);
        Debug.Log($"[PickUp] 🧹 Destroying pickup '{name}'");
        Destroy(gameObject);
    }

    protected abstract void OnPickUp(GameObject player);
}
