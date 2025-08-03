// Fire.cs
public class Fire : HazardBase, IObstacle
{
    public ObstacleType Type => ObstacleType.Fire;
    public int ContactDamage => 999;   // “lose life”
    public int RidingDamage  => 0;     // only the animal is lost
    public void DestroyObstacle() => Destroy(gameObject);
}
