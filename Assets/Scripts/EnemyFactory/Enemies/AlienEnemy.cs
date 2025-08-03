using UnityEngine;

public class AlienEnemy : ShadowEnemy
{
    void Awake()
    {
        Debug.Log("Alien is Alive!!!");
    }

    public override void Attack()
    {
        Debug.Log("Alien is Attacking!!!");
    }
}
