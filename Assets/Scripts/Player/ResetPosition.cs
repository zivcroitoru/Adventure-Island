using UnityEngine;

public class ResetPosition : MonoBehaviour, IResettable
{
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
        GameResetManager.Instance?.Register(this);
    }

    public void ResetPlayerPosition()
    {
        transform.position = _startPosition;
    }

    // Implement IResettable
    public void ResetState()
    {
        ResetPlayerPosition();
        Debug.Log("[ResetPosition] ResetState called: position reset.");
    }
}
