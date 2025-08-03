public interface IObstacle
{
    int ContactDamage { get; }   // player on foot
    int RidingDamage  { get; }   // player riding an animal
    void DestroyObstacle();      // let the obstacle clean itself up
}
