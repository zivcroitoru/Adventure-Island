using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour, IPickable
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Collect(col.gameObject); // renamed method
            gameObject.SetActive(false);
        }
    }

    public void Collect(GameObject target)
    {
        OnPickUp(target);
    }

    protected abstract void OnPickUp(GameObject player);
}
