using UnityEngine;

public class SoldierEnemy : ShadowEnemy
{
    void Awake()
    {
        Debug.Log("Soldier is Alive!!!");
    }

    public override void Attack()
    {
        Debug.Log("Soldier is Attacking!!!");
    }
}
