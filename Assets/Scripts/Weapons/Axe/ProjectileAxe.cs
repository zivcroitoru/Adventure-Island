using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class ProjectileAxe : BaseProjectile
{
    [SerializeField] private float ySpeed = 5f;
    [SerializeField] private float spinSpeed = 3000f;

    private Rigidbody2D _rb;

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1f, 1f);

        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(_speed * dir.x, ySpeed), ForceMode2D.Impulse);
    }

    void Update() => transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

    /* IPoolable */
    public override void OnSpawn()
    {
        Debug.Log("[ProjectileAxe] OnSpawn");
    }

    public override void ResetState()
    {
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
    }
}
