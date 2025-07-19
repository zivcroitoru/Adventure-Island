using System.Collections;
using UnityEngine;

public class MovingTile2D : MonoBehaviour
{
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float moveTime = 1f;

    private Vector3 startPos;
    private Vector3 rightPos;
    private Vector3 leftPos;

    private void Start()
    {
        startPos = transform.position;
        rightPos = startPos + Vector3.right * tileSize * 2;
        leftPos = startPos + Vector3.left * tileSize * 2;

        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return MoveTo(rightPos);  // Move right
            yield return MoveTo(leftPos);   // Then move left
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 initial = transform.position;
        float elapsed = 0f;

        while (elapsed < moveTime)
        {
            transform.position = Vector3.Lerp(initial, target, elapsed / moveTime); // Smooth movement
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Snap exactly to target at the end
    }
}
