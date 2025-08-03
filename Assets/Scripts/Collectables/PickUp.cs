using UnityEngine;

public abstract class PickUp : MonoBehaviour, IPickable
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        Collect(col.gameObject);
    }

    public void Collect(GameObject target)
    {
        OnPickUp(target);
        Destroy(gameObject); // ✅ clean up
    }

    protected abstract void OnPickUp(GameObject player);
}
