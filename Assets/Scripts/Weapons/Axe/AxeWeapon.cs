using VContainer;
using UnityEngine;

public class AxeWeapon : MonoBehaviour, IUseableWeapon
{
    private AxePoolManager _axePoolManager;
    private bool _isEquipped = false;

    [Inject]
    public void Construct(AxePoolManager axePoolManager)
    {
        _axePoolManager = axePoolManager;
    }

    public void Equip() => _isEquipped = true;
    public void UnEquip() => _isEquipped = false;

    public void Shoot()
    {
        if (!_isEquipped || _axePoolManager == null)
        {
            Debug.LogWarning("[AxeWeapon] Shoot called but either not equipped or poolManager is null.");
            return;
        }

        GameObject axe = _axePoolManager.GetPooledAxe();
        if (axe == null)
        {
            Debug.LogError("[AxeWeapon] Could not get pooled axe!");
            return;
        }

        axe.transform.position = transform.position;
        axe.SetActive(true);

        if (axe.TryGetComponent(out ProjectileAxe proj))
        {
            float direction = transform.localScale.x >= 0 ? 1f : -1f;
            proj.Shoot(direction);
        }
        else
        {
            Debug.LogError("[AxeWeapon] Axe GameObject missing ProjectileAxe component.");
        }
    }
}
