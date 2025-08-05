using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class ProjectileAxe : BaseProjectile
{
    [SerializeField] private float lifetime = 3f;

    public override void Shoot(Vector2 origin, Vector2 dir, float playerSpeed = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;

        // Optional: add velocity, spin, animation logic
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public override void OnDespawn()
    {
        CancelInvoke();
        // Optional: clear trail, stop effects
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }
}
