// using UnityEngine;

// public class PlayerController : MonoBehaviour, IDamageable
// {
//     [SerializeField] private GameObject mountObject; // Reference to mount (optional)
//     [SerializeField] private float health = 3f;

//     public bool HasMount => mountObject != null && mountObject.activeSelf;

//     public void Dismount()
//     {
//         if (mountObject != null)
//         {
//             mountObject.SetActive(false);
//             Debug.Log("[PlayerController] Dismounted.");
//         }
//     }

//     public void TakeDamage(int amount, Vector3 hitDirection)
//     {
//         health -= amount;
//         Debug.Log($"[PlayerController] Took {amount} damage. Remaining health: {health}");

//         if (health <= 0)
//         {
//             Die();
//         }
//     }

//     private void Die()
//     {
//         Debug.Log("[PlayerController] Player died.");
//         // TODO: Respawn or reload level
//     }
// }
