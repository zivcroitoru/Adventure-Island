using UnityEngine;

public sealed class GreenAnimal : AnimalBase
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 180f;
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
        SetRiderInvincible(true);

        Invoke(nameof(EndSpin), spinDuration);
        PerformSpinAttack();
    }

    private void PerformSpinAttack()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, spinRadius);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject || hit.gameObject == Rider)
                continue;

            if (hit.TryGetComponent<IObstacle>(out var obstacle) && CanDestroy(obstacle.Type))
            {
                obstacle.DestroyObstacle();
                continue;
            }

            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(1);
            }
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
            inv.SetTemporaryInvincibility(state);
        }
    }

    public override void Dismount()
    {
        if (_isSpinning) return;
        base.Dismount();
    }
}
