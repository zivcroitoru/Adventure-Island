using UnityEngine;

public class EnemyHeadTrigger : MonoBehaviour
{
    private ShadowEnemy parentEnemy;

    void Start()
    {
        parentEnemy = GetComponentInParent<ShadowEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.y < 0f)
            {
                parentEnemy.Die(); // call base logic
                rb.velocity = new Vector2(rb.velocity.x, 5f); // bounce player up
            }
        }
    }
}
