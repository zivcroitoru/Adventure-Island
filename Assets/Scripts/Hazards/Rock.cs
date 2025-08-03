// Rock.cs
public class Rock : HazardBase, IObstacle
{
    public int ContactDamage => 3;
    public int RidingDamage  => 3;
    public void DestroyObstacle() => Destroy(gameObject);
}
