using UnityEngine;

public class BlueAnimal : AnimalBase
{
    protected override void OnAttack()
    {
        TriggerAttackHitbox();
        Debug.Log("[BlueAnimal] Tail whip.");
    }

    public override bool CanDestroy(ObstacleType type)
    {
        return type == ObstacleType.Rock;
    }
}
