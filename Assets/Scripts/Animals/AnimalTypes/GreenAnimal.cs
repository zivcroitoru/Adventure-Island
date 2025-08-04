using UnityEngine;

public class GreenAnimal : AnimalBase
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 180f;
    [SerializeField] private float spinRadius = 0.6f;
    [SerializeField] private float spinDuration = 0.3f;

    private bool isSpinning = false;
    public bool IsSpinning => isSpinning;

    private void Update()
    {
        if (Rider == null)
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }

    public override void OnCollect(GameObject player)
    {
        Debug.Log("[GreenAnimal] Collected by player.");
        base.OnCollect(player);
    }

    protected override void OnAttack()
    {
        if (isSpinning) return;

        isSpinning = true;
        Invoke(nameof(EndSpin), spinDuration);
        PerformSpinAttack();
    }

    private void PerformSpinAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, spinRadius);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject || hit.gameObject == Rider)
                continue;

            if (hit.TryGetComponent<IObstacle>(out var obstacle) && CanDestroy(obstacle.Type))
            {
                Debug.Log("[GreenAnimal] 💥 Spin destroyed obstacle!");
                obstacle.DestroyObstacle();
                continue;
            }

            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                Debug.Log("[GreenAnimal] 💥 Spin damaged enemy!");

                if (damageable is Component comp)
                    Debug.Log($"[GreenAnimal] Damaging: {comp.gameObject.name}");

                damageable.TakeDamage(1);
            }
        }
    }

    private void EndSpin()
    {
        isSpinning = false;
    }

    public override bool CanDestroy(ObstacleType type)
    {
        return type == ObstacleType.Rock;
    }

    public override void Dismount()
    {
        if (isSpinning)
        {
            Debug.Log("[GreenAnimal] 🚫 Blocked dismount while spinning.");
            return;
        }

        base.Dismount();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spinRadius);
    }
#endif
}
