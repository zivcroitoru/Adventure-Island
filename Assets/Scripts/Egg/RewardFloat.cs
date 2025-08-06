using UnityEngine;

[DisallowMultipleComponent]
public sealed class RewardFloat : MonoBehaviour
{
    [SerializeField] private float floatHeight = 1f;
    [SerializeField] private float floatDuration = 0.5f;
    [SerializeField] private Collider2D pickupCollider;

    private void Awake()
    {
        if (pickupCollider != null)
            pickupCollider.enabled = false; // prevent early pickup
    }

    private void OnEnable() => StartCoroutine(FloatUp());

    private System.Collections.IEnumerator FloatUp()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * floatHeight;
        float time = 0f;

        while (time < floatDuration)
        {
            transform.position = Vector3.Lerp(start, end, time / floatDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        if (pickupCollider != null)
            pickupCollider.enabled = true; // now it can be collected
    }
}
