using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class RedAnimal : AnimalBase
{
    [Header("Fire-Spit Settings")]
    [SerializeField] private float projectileOffset = 0.4f;

    private ProjectileFirePool _firePool;

    [Inject]
    public void Construct(ProjectileFirePool firePool)
    {
        _firePool = firePool;
    }

    protected override void OnAttack()
    {
        Vector2 direction = GetFacingDirection();
        Vector2 spawnPos = transform.position + (Vector3)(direction * projectileOffset);

        var fire = _firePool.Get(spawnPos, Quaternion.identity);
        if (fire == null) return;

        fire.Shoot(spawnPos, direction, GetPlayerSpeed());
    }
}
