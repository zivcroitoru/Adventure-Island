using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileAxe : MonoBehaviour
{
    [Header("Movement Settings")]
    public float xSpeed = 5f;
    public float ySpeed = 5f;
    public float spinSpeed = 3000f;
    public Vector2 playerVelocity;

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    // Layer cache
    private int _layerPlayer;
    private int _layerPickable;
    private int _layerEnemy;
    private int _layerGround;
    private int _layerHazard;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _layerPlayer = LayerMask.NameToLayer("Player");
        _layerPickable = LayerMask.NameToLayer("Pickable");
        _layerEnemy = LayerMask.NameToLayer("Enemy");
        _layerGround = LayerMask.NameToLayer("Ground");
        _layerHazard = LayerMask.NameToLayer("Hazard");

        Debug.Log("[ProjectileAxe] Awake - Rigidbody and SpriteRenderer initialized");
    }

    private void OnEnable()
    {
        Debug.Log("[ProjectileAxe] OnEnable");

        if (_rigid != null)
        {
            _rigid.velocity = Vector2.zero;
            _rigid.angularVelocity = 0f;
        }

        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true;
            if (_spriteRenderer.sprite == null)
                Debug.LogWarning("[ProjectileAxe] SpriteRenderer has no sprite assigned!");
        }
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    public void Shoot(float direction)
    {
        Debug.Log($"[ProjectileAxe] Shoot() called. Direction: {direction}, PlayerVelocity: {playerVelocity}");

        if (_rigid == null)
        {
            Debug.LogError("[ProjectileAxe] Rigidbody2D is null in Shoot()");
            return;
        }

        transform.localScale = new Vector3(direction, 1f, 1f);

        Vector2 launchForce = new Vector2(xSpeed * direction, ySpeed) + new Vector2(playerVelocity.x, 0);
        _rigid.AddForce(launchForce, ForceMode2D.Impulse);

        Debug.Log($"[ProjectileAxe] Launch force: {launchForce}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (otherLayer == _layerPlayer || otherLayer == _layerPickable)
            return;

        if (otherLayer == _layerEnemy)
        {
            Debug.Log("[ProjectileAxe] Hit enemy!");
            Destroy(other.gameObject); // Replace with enemy.TakeDamage() if needed
        }

        if (otherLayer == _layerEnemy || otherLayer == _layerGround || otherLayer == _layerHazard)
        {
            Debug.Log($"[ProjectileAxe] Triggered with {LayerMask.LayerToName(otherLayer)}, deactivating.");
            gameObject.SetActive(false);
        }
    }
}
