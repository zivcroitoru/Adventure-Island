using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageDealer))]
public sealed class Fire : MonoBehaviour, IObstacle, IResettable
{
    [Header("Damage")]
    [SerializeField] private int contactDamage = 999;
    [SerializeField] private int ridingDamage  = 0;

    [Header("Setup")]
    [SerializeField] private bool forceTrigger = true;

    public ObstacleType Type         => ObstacleType.Fire;
    public int          ContactDamage => contactDamage;
    public int          RidingDamage  => ridingDamage;

    void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col && forceTrigger) col.isTrigger = true;
    }
void OnDisable(){ Debug.LogWarning($"[Fire] OnDisable {name} root={transform.root.name}"); }
void OnDestroy(){ Debug.LogError($"[Fire] OnDestroy {name}"); }

    // Only destroy if the instigator exists AND is invincible
public void DestroyObstacle()
{
    Debug.LogError($"[FIRE] DestroyObstacle CALLED by: {name}. Stack:\n{Environment.StackTrace}");
    Destroy(gameObject);
}



    public void ResetState()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        var col = GetComponent<Collider2D>(); if (col) col.enabled = true;
        var sr  = GetComponentInChildren<SpriteRenderer>(true); if (sr) sr.enabled = true;
    }
}
