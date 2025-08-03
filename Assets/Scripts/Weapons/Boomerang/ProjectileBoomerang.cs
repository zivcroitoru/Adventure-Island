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
        if (_isReturning && other.CompareTag("Player"))
        {
            OnReturned?.Invoke(); // ✅ Notify weapon to allow reuse
            gameObject.SetActive(false); // Return to pool or destroy
        }

        // TODO: Add other hit responses (rocks, enemies, etc.)
    }
}
