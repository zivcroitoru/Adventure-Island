using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ProjectileAxe : BaseProjectile
{
    [Header("Arc Settings")]
    [SerializeField] private float ySpeed = 5f;
    [SerializeField] private float spinSpeed = 3000f;

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    private int _layerPlayer, _layerPickable, _layerGround, _layerHazard, _layerEnemy;

    private float _direction = 1f;
    public Vector2 PlayerVelocity { get; set; }

    private IAttacker _attacker;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _layerPlayer   = LayerMask.NameToLayer("Player");
        _layerPickable = LayerMask.NameToLayer("Pickable");
        _layerGround   = LayerMask.NameToLayer("Ground");
        _layerHazard   = LayerMask.NameToLayer("Hazard");
        _layerEnemy    = LayerMask.NameToLayer("Enemy");

        Debug.Log("[ProjectileAxe] Awake - Initialized.");
    }

    private void OnEnable()
    {
        ResetPhysics();
        ResetVisuals();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

public override void Shoot()
{
    if (_rigid == null)
    {
        Debug.LogError("[ProjectileAxe] Rigidbody2D is null");
        return;
    }

    transform.localScale = new Vector3(_direction, 1f, 1f);

    Vector2 launchForce = new Vector2(_speed * _direction, ySpeed);
    _rigid.AddForce(launchForce, ForceMode2D.Impulse);

    Debug.Log($"[ProjectileAxe] Shoot: force = {launchForce}");

    _attacker?.Attack();
}


    public void SetDirection(float dir) => _direction = dir;
    public void SetAttacker(IAttacker newAttacker) => _attacker = newAttacker;

    public override void Initialized(float speed)
    {
        base.Initialized(speed);
        PlayerVelocity = Vector2.zero;
        _direction = 1f;
    }

    private void ResetPhysics()
    {
        if (_rigid != null)
        {
            _rigid.velocity = Vector2.zero;
            _rigid.angularVelocity = 0f;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void ResetVisuals()
    {
        if (_spriteRenderer == null) return;

        _spriteRenderer.enabled = true;

        if (_spriteRenderer.sprite == null)
            Debug.LogWarning("[ProjectileAxe] SpriteRenderer has no sprite assigned!");
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        // Ignore Player and Pickables
        if (otherLayer == _layerPlayer || otherLayer == _layerPickable)
            return;

        // Try apply damage
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(1);
            Debug.Log($"[ProjectileAxe] Hit {other.name}, applied damage.");
        }

        // Deactivate if hitting ground, hazard, or enemy
        if (otherLayer == _layerGround || otherLayer == _layerHazard || otherLayer == _layerEnemy)
        {
            Debug.Log($"[ProjectileAxe] Hit {LayerMask.LayerToName(otherLayer)} - deactivating.");
            gameObject.SetActive(false);
        }
    }
}
