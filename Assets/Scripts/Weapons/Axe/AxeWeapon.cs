using UnityEngine;
using VContainer;

public class AxeWeapon : MonoBehaviour, IUseableWeapon
{
    [SerializeField] private float shootCooldown = 0.5f; // 🔁 Delay between throws in seconds
    private float _lastShootTime;

    private bool _isEquipped = false;
    private AxePoolManager _axePoolManager;

    [Inject]
    public void Construct(AxePoolManager axePoolManager)
    {
        _axePoolManager = axePoolManager;
    }

    public void Shoot()
    {
        if (!_isEquipped || _axePoolManager == null)
            return;

        // ⏱ Cooldown check
        if (Time.time < _lastShootTime + shootCooldown)
            return;

        _lastShootTime = Time.time;

        GameObject axeGO = _axePoolManager.GetPooledAxe();
        if (axeGO == null)
            return;

        axeGO.transform.position = transform.position;

        float direction = transform.parent != null && transform.parent.localScale.x < 0 ? -1f : 1f;
        axeGO.transform.localScale = new Vector3(direction, 1, 1);
        axeGO.SetActive(true);

        var projectile = axeGO.GetComponent<ProjectileAxe>();
        if (projectile != null)
        {
            Rigidbody2D playerRb = transform.parent?.GetComponent<Rigidbody2D>();
            projectile.playerVelocity = playerRb != null ? playerRb.velocity : Vector2.zero;

            projectile.Shoot(direction);
        }
    }

    public void Equip() => _isEquipped = true;
    public void UnEquip() => _isEquipped = false;
}
