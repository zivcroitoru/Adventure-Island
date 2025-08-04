using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileAxe : BaseProjectile
{
    [Header("Arc Settings")]
    [SerializeField] private float ySpeed = 5f;
    [SerializeField] private float spinSpeed = 3000f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private IAttacker attacker;

    private int layerPlayer, layerPickable, layerGround, layerHazard;

    private float direction;
    public Vector2 PlayerVelocity { get; set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        layerPlayer = LayerMask.NameToLayer("Player");
        layerPickable = LayerMask.NameToLayer("Pickable");
        layerGround = LayerMask.NameToLayer("Ground");
        layerHazard = LayerMask.NameToLayer("Hazard");

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
        Debug.Log($"[ProjectileAxe] Shoot() - Direction: {direction}, PlayerVelocity: {PlayerVelocity}");

        if (rigid == null)
        {
            Debug.LogError("[ProjectileAxe] Rigidbody2D is null");
            return;
        }

        transform.localScale = new Vector3(direction, 1f, 1f);

        Vector2 launchForce = new Vector2(_speed * direction, ySpeed) + new Vector2(PlayerVelocity.x, 0);
        rigid.AddForce(launchForce, ForceMode2D.Impulse);

        Debug.Log($"[ProjectileAxe] Launch force: {launchForce}");

        attacker?.Attack();
    }

    public void SetDirection(float dir) => direction = dir;
    public void SetAttacker(IAttacker newAttacker) => attacker = newAttacker;

    public override void Initialized(float speed)
    {
        base.Initialized(speed);
        PlayerVelocity = Vector2.zero;
        direction = 1f;
    }

    private void ResetPhysics()
    {
        if (rigid != null)
        {
            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0f;
        }

        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    private void ResetVisuals()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.enabled = true;

        if (spriteRenderer.sprite == null)
            Debug.LogWarning("[ProjectileAxe] SpriteRenderer has no sprite assigned!");
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (otherLayer == layerPlayer || otherLayer == layerPickable)
            return;

        base.OnTriggerEnter2D(other); // applies damage if IDamageable

        if (otherLayer == layerGround || otherLayer == layerHazard)
        {
            Debug.Log($"[ProjectileAxe] Triggered with {LayerMask.LayerToName(otherLayer)}, deactivating.");
            gameObject.SetActive(false);
        }
    }
}
