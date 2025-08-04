using UnityEngine;

public class VerticalMove : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float height = 1f;

    private float _originY;

    private void Start() => _originY = transform.position.y;

    public void Move(Transform t)
    {
        var y = _originY + Mathf.Sin(Time.time * speed) * height;
        t.position = new Vector3(t.position.x, y, t.position.z);
    }
}
