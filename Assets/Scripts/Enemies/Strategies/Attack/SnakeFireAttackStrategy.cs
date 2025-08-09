using UnityEngine;
using VContainer;

public class SnakeFireAttackStrategy : MonoBehaviour, IAttackStrategy
{
    [SerializeField] float fireCooldown = 2f;
    [SerializeField] Vector2 fireDirection = Vector2.left;

    [SerializeField] SnakeFireProjectilePool pool; // optional (can stay null)
    float _nextFireTime;

    [Inject] public void Construct(SnakeFireProjectilePool injected)
    {
        if (injected) pool = injected; // DI wins when present
    }

    void Awake()
    {
        if (!pool)
        {
            // Unity 2021+: FindFirstObjectByType; else use FindObjectOfType
#if UNITY_2021_3_OR_NEWER
            pool = FindFirstObjectByType<SnakeFireProjectilePool>(FindObjectsInactive.Include);
#else
            pool = FindObjectOfType<SnakeFireProjectilePool>(true);
#endif
        }
    }

    public void Attack()
    {
        if (Time.time < _nextFireTime || !pool) return;
        var proj = pool.Get(transform.position, Quaternion.identity);
        if (!proj) return;

        proj.Shoot(transform.position, fireDirection);
        _nextFireTime = Time.time + fireCooldown;
    }
}
