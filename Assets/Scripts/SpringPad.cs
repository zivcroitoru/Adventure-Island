using UnityEngine;

public class SpringPad : MonoBehaviour
{
    [SerializeField] private float springForce = 25f;
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * springForce, ForceMode2D.Impulse);

                // ✅ Trigger jump animation
                Animator animator = collision.collider.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
            }
        }
    }
}
