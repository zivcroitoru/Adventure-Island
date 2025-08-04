using UnityEngine;

public class TailWhipAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 0.2f;

    private void OnEnable()
    {
        Invoke(nameof(DisableSelf), lifetime);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TailWhip] Triggered by: {other.name}");

        if (other.TryGetComponent(out IObstacle obstacle))
        {
            if (CanDestroy(obstacle.Type))
            {
                Debug.Log("[TailWhip] Destroying obstacle");
                obstacle.DestroyObstacle();
            }
            else
            {
                Debug.Log("[TailWhip] Obstacle is NOT destroyable");
            }
        }

        // if (other.TryGetComponent(out IEnemy enemy))
        // {
        //     if (CanHurtEnemy(enemy.Type))
        //     {
        //         Debug.Log("[TailWhip] Damaging enemy");
        //         enemy.TakeDamage(damage);
        //     }
        //     else
        //     {
        //         Debug.Log("[TailWhip] Enemy is immune");
        //     }
        // }
    }

    private bool CanDestroy(ObstacleType type)
    {
        return type != ObstacleType.Fire;
    }

    private bool CanHurtEnemy(EnemyType type)
    {
        return type != EnemyType.Ghost;
    }
}
