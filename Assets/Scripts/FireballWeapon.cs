using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FireballWeapon : MonoBehaviour, IUseableWeapon
{
    public GameObject fireball;
    private bool _isEquip = false;
    [Inject] private FireballPoolManager _fireballPoolManager;

    public void Shoot()
    {
        if(fireball != null && _isEquip)
        {
            //GameObject curFireball = Instantiate(fireball,transform.position,new Quaternion());
            GameObject curFireball = _fireballPoolManager.GetPooledFireball();
            curFireball.transform.position = transform.position;
            curFireball.SetActive(true);

            ProjectileFireball scFireball = curFireball.GetComponent<ProjectileFireball>();
            if (scFireball != null)
            {
                float direction = 1;
                if (transform.parent != null)
                    direction = transform.parent.localScale.x;
                scFireball.Shoot(direction);
            }
        }
    }

    public void UnEquip()
    {
        _isEquip = false;
    }
    public void Equip()
    {
        _isEquip = true;
    }
}