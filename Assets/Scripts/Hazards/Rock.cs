public class Rock : HazardBase, IObstacle, IDamageable
{
    public ObstacleType Type => ObstacleType.Rock;
    public int ContactDamage => 3;
    public int RidingDamage  => 3;

    public void DestroyObstacle() => Destroy(gameObject);

    public void TakeDamage(int amount)
    {
        DestroyObstacle();
    }
}
