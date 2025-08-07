using UnityEngine;
using VContainer;

public sealed class BoomerangWeapon : BaseWeapon
{
    [Inject] private ProjectileBoomerangPool pool;
    private ProjectileBoomerang active;

    protected override void Fire()
    {
        if (active != null) return;

        Vector3 spawnPos = GetSpawnPosition();
        float dir = Mathf.Sign(transform.root.localScale.x);
        Vector2 direction = Vector2.right * dir;

        active = pool.Get(spawnPos, Quaternion.identity);
        active.SetPlayer(transform.root); // inject player reference
        active.Shoot(spawnPos, direction);

        active.OnReturned = () =>
        {
            pool.Release(active);
            active = null;
        };
    }

    private Vector3 GetSpawnPosition()
    {
        Transform root = transform.root;
        return new Vector3(
            root.position.x + 0.6f * Mathf.Sign(root.localScale.x),
            root.position.y,
            0f
        );
    }
}
