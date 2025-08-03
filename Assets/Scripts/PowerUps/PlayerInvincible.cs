using UnityEngine;
using System.Collections;

public class PlayerInvincible : MonoBehaviour, IInvincible
{
    private bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;

    public float powerUpDuration = 5f;
    private SpriteRenderer _curSpriteRenderer;

    private void Awake()
    {
        _curSpriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void ActivateInvincibility()
    {
        Debug.Log("ActivateInvincibility");
        StartCoroutine(ActivateInvincibilityTimer());
    }

    private IEnumerator ActivateInvincibilityTimer()
    {
        _isInvincible = true;
        _curSpriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(powerUpDuration);
        _curSpriteRenderer.color = Color.white;
        _isInvincible = false;
    }
}