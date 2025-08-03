using UnityEngine;

public abstract class ShadowEnemy : MonoBehaviour
{
    public GameObject deathDropPrefab;

    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 lastPosition;

    void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        Vector3 newPosition = startPosition + Vector3.right * (offset - moveDistance / 2);
        transform.position = newPosition;

        // Detect direction change
        float delta = newPosition.x - lastPosition.x;
        if (delta != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Sign(delta) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        lastPosition = newPosition;
    }

    public abstract void Attack();

    public virtual void Die()
    {
        SpawnDeathDrop();
        // FindObjectOfType<ScoreManager>()?.AddScore(100);
        Destroy(gameObject);
    }

    private void SpawnDeathDrop()
    {
        if (deathDropPrefab != null)
        {
            Instantiate(deathDropPrefab, transform.position, Quaternion.identity);
        }
    }
}
