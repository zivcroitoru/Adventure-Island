using UnityEngine;

public class BirdMove : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private float verticalAmplitude = 1f;

    private float _originY;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;          // remember full start position
        _originY  = _startPos.y;

        // Start looping reset
        StartCoroutine(ResetLoop());
    }

    public void Move(Transform t)
    {
        // Move left
        t.Translate(Vector2.left * horizontalSpeed * Time.deltaTime);

        // Vertical wave
        float y = _originY + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        t.position = new Vector3(t.position.x, y, t.position.z);
    }

    private System.Collections.IEnumerator ResetLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            transform.position = _startPos;
            _originY = _startPos.y;
        }
    }
}
