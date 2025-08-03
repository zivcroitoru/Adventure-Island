using UnityEngine;

public class BoomerangWeapon : BaseWeapon
{
    [SerializeField] private GameObject boomerangPrefab;

    private GameObject _activeBoomerang;

    protected override void Fire()
    {
        if (!CanFire()) return;

        Vector3 spawnPos = GetSpawnPosition();
        _activeBoomerang = Instantiate(boomerangPrefab, spawnPos, transform.rotation);

        if (_activeBoomerang.TryGetComponent(out ProjectileBoomerang projectile))
        {
            Transform playerTransform = transform.root;

            // Get direction from player's facing (scale.x)
            float direction = Mathf.Sign(playerTransform.localScale.x);

            projectile.Shoot(playerTransform, direction);
        }
        else
        {
            Debug.LogError("[BoomerangWeapon] Missing ProjectileBoomerang component on prefab.");
        }
    }

    private bool CanFire()
    {
        if (_activeBoomerang != null && _activeBoomerang.activeInHierarchy)
        {
            Debug.Log("[BoomerangWeapon] Boomerang already active.");
            return false;
        }

        if (boomerangPrefab == null)
        {
            Debug.LogError("[BoomerangWeapon] Boomerang prefab is not assigned!");
            return false;
        }

        return true;
    }

    private Vector3 GetSpawnPosition()
    {
        Transform parent = transform.parent;
        Vector3 pos = parent ? parent.position : transform.position;
        pos.z = 0f;
        return pos;
    }
}
