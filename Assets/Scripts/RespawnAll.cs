using UnityEngine;

public class RespawnAll : MonoBehaviour
{
    public void Respawn()
    {
        var root = GameObject.Find("001");
        if (root == null) return;

        foreach (var pickable in root.GetComponentsInChildren<Transform>(true))
        {
            if (pickable.CompareTag("Pickable"))
                pickable.gameObject.SetActive(true);
        }

        Debug.Log("✅ Pickables under '001' reactivated.");
    }
}
