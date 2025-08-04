using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBoomerang : BaseProjectile
{
    [Header("Boomerang Settings")]
    [SerializeField] private float returnTime = 1.2f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float spinSpeed = 720f;

    private Rigidbody2D _rigidbody;
    private Transform _playerTransform;
    private bool _isReturning;

    public Action OnReturned; // 🔁 Callback to notify weapon

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector2.zero;
        _isReturning = false;
    }

protected override void OnDisable()
{
    CancelInvoke();
    OnReturned = null;
}

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        if (_isReturning && _playerTransform != null)
        {
            Vector2 toPlayer = (_playerTransform.position - transform.position).normalized;
            _rigidbody.velocity = toPlayer * speed;
        }
    }

    public override void Shoot()
    {
        Shoot(null, 1f);
    }

    public void Shoot(Transform player, float direction)
    {
        _playerTransform = player;

        Vector2 shootDir = Vector2.right * direction;
        _rigidbody.velocity = shootDir * speed;

        Invoke(nameof(StartReturn), returnTime);
    }

    private void StartReturn() => _isReturning = true;

protected override void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log($"[ProjectileBoomerang] Triggered with {other.name}, isReturning={_isReturning}");

    // 🔁 Return to player
    if (_isReturning && other.CompareTag("Player"))
    {
        Debug.Log("[ProjectileBoomerang] Returning to player — deactivating.");
        OnReturned?.Invoke();
        gameObject.SetActive(false);
        return;
    }

    // 💥 Hit destructible object
if (other.TryGetComponent(out IObstacle obstacle) && obstacle.Type == ObstacleType.Rock)
{
    Debug.Log($"[ProjectileBoomerang] Hit Rock — destroying.");
    obstacle.DestroyObstacle();
    gameObject.SetActive(false);
}


    // 🎯 Optional: damage enemy if needed
    if (other.TryGetComponent(out IDamageable target))
    {
        Debug.Log($"[ProjectileBoomerang] Hit IDamageable ({other.name}) — applying damage.");
        target.TakeDamage(1);
    }
}


}
