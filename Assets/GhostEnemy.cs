using UnityEngine;

[DisallowMultipleComponent]
public sealed class GhostEnemy : EnemyController
{
    private IInvincible _playerInv;

    protected override void Awake()
    {
        base.Awake();
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerInv = player ? player.GetComponentInChildren<IInvincible>() : null;
    }

    private bool PlayerIsInvincible => _playerInv != null && _playerInv.IsInvincible;

    // Called by the pipeline that passes a dealer
    public override void TakeDamage(int amount, GameObject dealer)
    {
        if (PlayerIsInvincible)
            ApplyDamage(int.MaxValue); // or just Die();
        // else ignore
    }

    // Safety: if anyone calls the overload without dealer
    public override void TakeDamage(int amount)
    {
        if (PlayerIsInvincible)
            ApplyDamage(int.MaxValue);
        // else ignore
    }
}
