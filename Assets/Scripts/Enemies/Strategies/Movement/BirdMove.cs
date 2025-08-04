using UnityEngine;

public class BirdMove : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private float verticalAmplitude = 1f;

    private float _originY;

    private void Start()
    {
        _originY = transform.position.y;
    }

    public void Move(Transform t)
    {
        // Move left
        t.Translate(Vector2.left * horizontalSpeed * Time.deltaTime);

        // Vertical wave
        float y = _originY + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        t.position = new Vector3(t.position.x, y, t.position.z);
    }
}
