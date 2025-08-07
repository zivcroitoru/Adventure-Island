using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : EnemyBase
{
    [SerializeField] private MonoBehaviour movementStrategy; // Must implement IMovementStrategy
    [SerializeField] private MonoBehaviour attackStrategy;   // Must implement IAttackStrategy

    private IMovementStrategy _move;
    private IAttackStrategy _attack;


protected override void Awake()
{
    Debug.Log("[EnemyController] Awake on " + gameObject.name);

    base.Awake();

    _move = movementStrategy as IMovementStrategy;
    _attack = attackStrategy as IAttackStrategy;
}



    private void Update()
    {
        _move?.Move(transform);
        _attack?.Attack();
    }
}
