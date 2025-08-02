using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public static WeaponUI Instance; // Global access

    public Image weaponIcon;

    public Sprite flashShotSprite;
    public Sprite lightShotSprite;
    public Sprite subMachineGunSprite;
    public Sprite pistolSprite;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Optional: destroy duplicates
    }

    public void UpdateWeaponIcon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.FlashShot:
                weaponIcon.sprite = flashShotSprite;
                break;
            case WeaponType.LightShot:
                weaponIcon.sprite = lightShotSprite;
                break;
            case WeaponType.SubMachineGun:
                weaponIcon.sprite = subMachineGunSprite;
                break;
            case WeaponType.Pistol:
                weaponIcon.sprite = pistolSprite;
                break;
            default:
                weaponIcon.sprite = null;
                break;
        }
    }
}
