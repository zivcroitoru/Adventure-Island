using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileAxe : MonoBehaviour
{
    public float xSpeed = 5f;
    public float ySpeed = 5f;
    public float destroyTime = 5f;
    public float spinSpeed = 720f;
    public Vector2 playerVelocity; // 🟢 Assigned externally before shooting

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (_rigid != null)
        {
            _rigid.velocity = Vector2.zero;
            _rigid.angularVelocity = 0f;
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    public void Shoot(float direction)
    {
        if (_rigid == null) return;

        transform.localScale = new Vector3(direction, 1, 1);

        Vector2 launchForce = new Vector2(xSpeed * direction, ySpeed) + playerVelocity;
        _rigid.AddForce(launchForce, ForceMode2D.Impulse);

        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
    }
}
