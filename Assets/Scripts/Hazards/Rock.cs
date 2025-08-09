using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public sealed class Rock : MonoBehaviour, IObstacle
{
    [SerializeField] private int contactDamage = 3;
    [SerializeField] private int ridingDamage  = 1;
    [SerializeField] private ObstacleType type = ObstacleType.Rock;

    public int ContactDamage => contactDamage;
    public int RidingDamage  => ridingDamage;
    public ObstacleType Type => type;

    public void DestroyObstacle() => Destroy(gameObject);

    private void OnTriggerEnter2D(Collider2D other) =>
        DealContactDamage(other.attachedRigidbody?.gameObject ?? other.gameObject);

    private void OnCollisionEnter2D(Collision2D col) =>
        DealContactDamage(col.rigidbody?.gameObject ?? col.collider.gameObject);

    private void DealContactDamage(GameObject hit)
    {
        if (!hit) return;
        var rider = hit.GetComponentInParent<AnimalBase>()?.Rider;
        var target = rider ?? hit;

        // Ignore self/same root
        if (target.transform.root == transform.root) return;

        int dmg = rider ? ridingDamage : contactDamage;
        Damage.Deal(dmg, gameObject, target);
    }
}
