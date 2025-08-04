using UnityEngine;

public class EnemyController : EnemyBase
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private MonoBehaviour movementStrategy; // IMovementStrategy
    [SerializeField] private MonoBehaviour attackStrategy;   // IAttackStrategy

    private IMovementStrategy _move;
    private IAttackStrategy _attack;

    private void Awake()
    {
        _move = movementStrategy as IMovementStrategy;
        _attack = attackStrategy as IAttackStrategy;
    }

    private void Update()
    {
        _move?.Move(transform);
        _attack?.Attack();
    }
}
