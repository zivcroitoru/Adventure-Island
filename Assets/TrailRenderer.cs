using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float interval = 0.05f;
    public float fadeTime = 0.3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            SpawnGhost();
            timer = 0;
        }
    }

    void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        sr.sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(FadeOut(ghost, fadeTime));
    }

    IEnumerator FadeOut(GameObject ghost, float duration)
    {
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        Color original = sr.color;
        float timer = 0;
        while (timer < duration)
        {
            float alpha = Mathf.Lerp(original.a, 0, timer / duration);
            sr.color = new Color(original.r, original.g, original.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(ghost);
    }
}
