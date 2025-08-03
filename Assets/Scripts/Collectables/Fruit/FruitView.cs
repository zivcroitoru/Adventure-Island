using UnityEngine;

public class FruitView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null && sprite != null)
            spriteRenderer.sprite = sprite;
    }
}
