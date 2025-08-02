using System.Collections;
using UnityEngine;

public class ProjectileAxe : MonoBehaviour
{
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public float destroyTime = 5f;
    public float spinSpeed = 720f; // Degrees per second

    private Rigidbody2D _rigid;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    public void Shoot(float direction)
    {
        if (_rigid != null)
        {
            transform.localScale = new Vector3(direction, 1, 1);
            _rigid.AddForce(new Vector3(xSpeed * direction, ySpeed, 0));
            StartCoroutine(DestroyObject());
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
