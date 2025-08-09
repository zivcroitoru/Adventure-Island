using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public sealed class InvincibleRammer : MonoBehaviour
{
    [Header("Layers (exact names)")]
    [SerializeField] string enemyLayerName  = "Enemy";
    [SerializeField] string hazardLayerName = "Hazard";

    [Header("Damage")]
    [SerializeField] int lethalTouchDamage = int.MaxValue;

    [Header("Don’t break during TEMP")]
    [SerializeField] ObstacleType[] dontBreak = { ObstacleType.Fire };

    [Header("Source (optional)")]
    [SerializeField] MonoBehaviour invSource; // FairyInvinciblePowerUp

    [Header("Debug")]
    [SerializeField] bool debugLogs = true;

    IInvincible inv;
    FairyInvinciblePowerUp fairy;
    Collider2D col;
    ContactFilter2D filter;
    HashSet<ObstacleType> deny;
    readonly List<Collider2D> hits = new(16);
    float suppressUntil;
    bool wasActive;

    public void SuppressFor(float sec) => suppressUntil = Mathf.Max(suppressUntil, Time.time + Mathf.Max(0f, sec));

    void Awake()
    {
        col = GetComponent<Collider2D>();
        inv = (invSource as IInvincible) ?? GetComponent<IInvincible>() ?? GetComponentInChildren<IInvincible>(true);
        fairy = inv as FairyInvinciblePowerUp;

        int enemy  = LayerMask.NameToLayer(enemyLayerName);
        int hazard = LayerMask.NameToLayer(hazardLayerName);
        int mask   = ((enemy >= 0 ? 1 << enemy : 0) | (hazard >= 0 ? 1 << hazard : 0));

        filter = new ContactFilter2D { useLayerMask = true, layerMask = mask, useTriggers = true };
        deny = new HashSet<ObstacleType>(dontBreak);

        if (debugLogs)
        {
            Debug.Log($"[InvincibleRammer] Ready. mask=0x{mask:X} enemy='{enemyLayerName}' hazard='{hazardLayerName}'", this);
            if (mask == 0) Debug.LogWarning("[InvincibleRammer] Layer mask is 0. Check layer names.", this);
        }
    }

    void FixedUpdate()
    {
        if (inv == null || !inv.IsInvincible) { Toggle(false); return; }

        bool powerUp = fairy && fairy.IsPowerUpActive;
        bool temp    = fairy && fairy.IsTempActive;
        bool active  = powerUp || (temp && Time.time >= suppressUntil);
        Toggle(active);
        if (!active) return;

        hits.Clear();
        int count = col.OverlapCollider(filter, hits);
        for (int i = 0; i < count; i++)
        {
            var h = hits[i]; if (!h) continue;
            var target = h.attachedRigidbody ? h.attachedRigidbody.gameObject : h.gameObject;
            if (!target || target.transform.root == transform.root) continue;

            if (target.TryGetComponent<IDamageable>(out var dmg) || (dmg = target.GetComponentInParent<IDamageable>()) != null)
            {
                if (debugLogs) Debug.Log($"[InvincibleRammer] LETHAL → {target.name}", target);
                dmg.TakeDamage(lethalTouchDamage, gameObject);
                continue;
            }

            if (target.TryGetComponent<IObstacle>(out var obs) || (obs = target.GetComponentInParent<IObstacle>()) != null)
            {
                bool denyTemp = temp && !powerUp && deny.Contains(obs.Type);
                if (denyTemp) { if (debugLogs) Debug.Log($"[InvincibleRammer] SKIP (TEMP) → {((Component)obs).gameObject.name} [{obs.Type}]"); continue; }
                if (debugLogs) Debug.Log($"[InvincibleRammer] BREAK → {((Component)obs).gameObject.name} [{obs.Type}]", ((Component)obs).gameObject);
                obs.DestroyObstacle();
            }
        }
    }

    void Toggle(bool on)
    {
        if (on == wasActive) return;
        if (debugLogs) Debug.Log(on ? "[InvincibleRammer] ACTIVE" : "[InvincibleRammer] INACTIVE", this);
        wasActive = on;
    }
}
