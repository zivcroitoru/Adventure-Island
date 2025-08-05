using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class ProjectileBoomerang : BaseProjectile
{
    [Header("Boomerang Settings")]
    [SerializeField] private float returnDelay = 1.2f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float spinSpeed = 720f;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private bool _isReturning;

    public Action OnReturned; // 🔁 Callback to notify weapon

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    private void OnEnable()
    {
        _rb.velocity = Vector2.zero;
        _isReturning = false;
    }

    private void OnDisable()
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
            _rb.velocity = toPlayer * speed;
        }
    }

    /* ---------- Launch ---------- */

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        _playerTransform = null;
        transform.position = origin;
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1f, 1f);
        _rb.velocity = dir.normalized * speed;
        Invoke(nameof(StartReturn), returnDelay);
    }

    public void Shoot(Transform player, float direction)
    {
        _playerTransform = player;
        transform.position = player.position;
        transform.localScale = new Vector3(Mathf.Sign(direction), 1f, 1f);
        _rb.velocity = Vector2.right * direction * speed;
        Invoke(nameof(StartReturn), returnDelay);
    }

    private void StartReturn()
    {
        _isReturning = true;
    }

    /* ---------- Collision ---------- */

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Boomerang] Hit {other.name}, returning = {_isReturning}");

        if (_isReturning && other.CompareTag("Player"))
        {
            Debug.Log("[Boomerang] Returned to player.");
            OnReturned?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        if (other.TryGetComponent(out IObstacle obstacle) && obstacle.Type == ObstacleType.Rock)
        {
            obstacle.DestroyObstacle();
            gameObject.SetActive(false);
            return;
        }

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }

    /* ---------- Reset ---------- */

    public override void ResetState()
    {
        CancelInvoke();
        _rb.velocity = Vector2.zero;
        _isReturning = false;
        _playerTransform = null;
    }
}
