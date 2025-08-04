using UnityEngine;

public class FireballAttack : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float interval = 2f;
    [SerializeField] private float speed = 5f;

    private float _nextTime;

    public void Attack()
    {
        if (Time.time < _nextTime) return;

        var fb = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fb.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        _nextTime = Time.time + interval;
    }
}
