using UnityEngine;

public class GreenAnimal : AnimalBase
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 180f;
    [SerializeField] private float spinRadius = 0.6f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float spinDuration = 0.3f;

    private bool isSpinning = false;
    public bool IsSpinning => isSpinning;

    private void Update()
    {
        if (Rider == null)
        {
            RotateIdle();
        }
    }

    private void RotateIdle()
    {
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
        ExecuteSpinAttack();
        Invoke(nameof(EndSpin), spinDuration);
    }

    private void ExecuteSpinAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, spinRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IObstacle obstacle) && CanDestroy(obstacle.Type))
            {
                Debug.Log("[GreenAnimal] 💥 Spin destroyed obstacle!");
                obstacle.DestroyObstacle();
                continue;
            }

            if (hit.TryGetComponent(out IDamageable enemy))
            {
                Debug.Log("[GreenAnimal] 💥 Spin hit enemy!");
                enemy.TakeDamage(damage);
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
