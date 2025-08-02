using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public FireballWeapon fireballWeapon;
    public AxeWeapon axeWeapon;
    public LaserWeapon laserWeapon;

    private Animator animator;

    void Start()
    {
        animator = transform.parent.Find("VisualShadow")?.GetComponent<Animator>();
    }

    void Update()
    {
        ActivateWeapon();
    }

    void SetShootTrigger()
    {
        // animator.ResetTrigger("Shoot");
        // animator.SetTrigger("Shoot");
    }

    private void ActivateWeapon()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && fireballWeapon != null)
        {
            fireballWeapon.Shoot();
            SetShootTrigger();
        }

        if (Input.GetKeyDown(KeyCode.Z) && axeWeapon != null)
        {
            axeWeapon.Reload();
        }

        if (Input.GetKeyDown(KeyCode.X) && axeWeapon != null)
        {
            axeWeapon.Shoot();
            SetShootTrigger();
        }

        if (Input.GetKeyDown(KeyCode.C) && laserWeapon != null)
        {
            laserWeapon.Shoot();
            SetShootTrigger();
        }
    }
}
