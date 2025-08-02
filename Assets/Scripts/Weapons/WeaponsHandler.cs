using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public AxeWeapon axeWeapon;
    // public BoomerangWeapon boomerangWeapon;

    private Animator animator;

    void Start()
    {
        // animator = transform.parent.Find("VisualShadow")?.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShootEquippedWeapon();
            SetShootTrigger();
        }
    }

    void SetShootTrigger()
    {
        // animator.ResetTrigger("Shoot");
        // animator.SetTrigger("Shoot");
    }

    private void ShootEquippedWeapon()
    {
        if (axeWeapon != null)
        {
            axeWeapon.Shoot();
            return;
        }

        // if (boomerangWeapon != null && boomerangWeapon.IsEquipped())
        // {
        //     boomerangWeapon.Shoot();
        //     return;
        // }

        Debug.LogWarning("[WeaponsHandler] No weapon equipped.");
    }
}
