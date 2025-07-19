using System.Collections;
using UnityEngine;

public class TileBlink : MonoBehaviour
{
    private Renderer tileRenderer;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private float visibleTime = 2f;
    [SerializeField] private float invisibleTime = 2f;

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }

    private void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            tileRenderer.enabled = true;
            boxCollider2D.enabled = true;
            yield return new WaitForSeconds(visibleTime);

            tileRenderer.enabled = false;
            boxCollider2D.enabled = false;
            yield return new WaitForSeconds(invisibleTime);
        }
    }

}
