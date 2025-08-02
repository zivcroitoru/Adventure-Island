using System.Collections;
using UnityEngine;

public class ProjectileAxe : MonoBehaviour
{
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public float destroyTime = 5f;
    public float spinSpeed = 720f; // Degrees per second

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Reset velocity to avoid buildup when reused
        if (_rigid != null)
            _rigid.velocity = Vector2.zero;
    }

    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    public void Shoot(float direction)
    {
        if (_rigid != null)
        {
            transform.localScale = new Vector3(direction, 1, 1);
            _rigid.AddForce(new Vector3(xSpeed * direction, ySpeed, 0));
            StartCoroutine(DisableAfterDelay());
        }
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false); // ✅ use pooling-safe disable
    }
}
