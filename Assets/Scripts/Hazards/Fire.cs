public class Fire : HazardBase, IObstacle, IDamageable
{
    public ObstacleType Type => ObstacleType.Fire;
    public int ContactDamage => 999;   // “lose life”
    public int RidingDamage  => 0;     // only the animal is lost

    public void DestroyObstacle() => Destroy(gameObject);

    public void TakeDamage(int amount)
    {
        DestroyObstacle();
    }
}
