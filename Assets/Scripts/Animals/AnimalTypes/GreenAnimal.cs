using UnityEngine;

public sealed class GreenAnimal : AnimalBase
{
    [Header("Spin Settings")]
    [SerializeField] private float spinRadius = 0.6f;
    [SerializeField] private float spinDuration = 0.3f;

    private bool _isSpinning = false;
    public bool IsSpinning => _isSpinning;

    // Use 'new' keyword to hide the base 'Rider' property
    public new GameObject Rider => base.Rider;

protected override void OnAttack()
{
    if (_isSpinning) return;

    _isSpinning = true;
    SetRiderInvincible(true); // now uses timed temp

    Invoke(nameof(EndSpin), spinDuration);
    PerformSpinAttack();
}


private void PerformSpinAttack()
{
    var hits = Physics2D.OverlapCircleAll(transform.position, spinRadius);

    foreach (var hit in hits)
    {
        var target = hit.attachedRigidbody ? hit.attachedRigidbody.gameObject : hit.gameObject;

        // Skip self and rider
        if (target == gameObject || target == Rider)
            continue;

        // Obstacles the green animal can destroy
        if (target.TryGetComponent<IObstacle>(out var obstacle) && CanDestroy(obstacle.Type))
        {
            obstacle.DestroyObstacle();
            continue;
        }

        // Route everything else through central damage rules
        Damage.Deal(1, gameObject, target);
    }
}


    private void EndSpin()
    {
        _isSpinning = false;
        SetRiderInvincible(false);
    }

private void SetRiderInvincible(bool state)
{
    if (Rider != null && Rider.TryGetComponent<IInvincible>(out var inv))
    {
        if (inv is FairyInvinciblePowerUp fairy)
        {
            if (state)
                fairy.ActivateTemp(spinDuration); // auto-clears after spin
            else
                fairy.SetTemporaryInvincibility(false); // manual clear if needed
        }
        else
        {
            // fallback for generic IInvincible
            inv.SetTemporaryInvincibility(state);
        }
    }
}

    public override void Dismount()
    {
        if (_isSpinning) return;
        base.Dismount();
    }
}
