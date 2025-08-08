using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public sealed class InvincibleObstacleBreaker : MonoBehaviour
{
    [SerializeField] private int contactDamageWhenInvincible = 100;

    private IInvincible inv;

    private void Awake()
    {
        inv = GetComponent<IInvincible>() ?? GetComponentInParent<IInvincible>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        TryBreak(target);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var target = col.rigidbody ? col.rigidbody.gameObject : col.collider.gameObject;
        TryBreak(target);
    }

    private void TryBreak(GameObject hit)
    {
        if (hit == null || inv == null || !inv.IsInvincible) return;
        if (ReferenceEquals(hit, gameObject)) return;
        if (hit.transform.root == transform.root) return;

        // Prefer rigidbody root
        var rb = hit.GetComponent<Rigidbody2D>() ?? hit.GetComponentInParent<Rigidbody2D>();
        var target = rb ? rb.gameObject : hit;

        // 1) If it's an obstacle, break it outright
        var obstacle = target.GetComponent<IObstacle>() ?? target.GetComponentInParent<IObstacle>();
        if (obstacle != null)
        {
            SafeDestroyObstacle(obstacle, target);
            return;
        }

        // 2) If it's damageable, hit it hard
        var damageable = target.GetComponent<IDamageable>() ?? target.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            Damage.Deal(contactDamageWhenInvincible, gameObject, target);
        }
    }

    /// <summary>
    /// Safe wrapper to avoid NREs from badly implemented DestroyObstacle().
    /// </summary>
    private void SafeDestroyObstacle(IObstacle obstacle, GameObject target)
    {
        try
        {
            obstacle.DestroyObstacle();
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[Breaker] DestroyObstacle on {target.name} threw: {ex.Message}", target);
            // fallback if needed
            target.SetActive(false);
        }
    }
}
