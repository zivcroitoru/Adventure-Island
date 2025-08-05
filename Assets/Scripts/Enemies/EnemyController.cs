using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : EnemyBase
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private MonoBehaviour movementStrategy; // Must implement IMovementStrategy
    [SerializeField] private MonoBehaviour attackStrategy;   // Must implement IAttackStrategy

    private IMovementStrategy _move;
    private IAttackStrategy _attack;
    public EnemyType Type => enemyType;


    private void Awake()
    {
        _move = movementStrategy as IMovementStrategy;
        _attack = attackStrategy as IAttackStrategy;

        if (_move == null) Debug.LogError("[EnemyController] Movement strategy is not valid.");
        if (_attack == null) Debug.LogError("[EnemyController] Attack strategy is not valid.");
    }

    private void Update()
    {
        _move?.Move(transform);
        _attack?.Attack();
    }
}
