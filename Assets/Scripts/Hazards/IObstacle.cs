public interface IObstacle
{
    int ContactDamage { get; }     // Damage to player on foot
    int RidingDamage { get; }      // Damage to player while riding
    ObstacleType Type { get; }     // Obstacle category
    void DestroyObstacle();        // Self-cleanup logic
}
