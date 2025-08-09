// AutoIFramesOnDamage.cs
// Attach to the Player. Subscribes to EnergyController.OnDamageTaken
// and grants 0.5 s invincibility on ANY damage. Uses Fairy if present,
// otherwise falls back to an internal temporary invincible helper.

using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public sealed class AutoIFramesOnDamage : MonoBehaviour
{
    [SerializeField] private float seconds = 0.5f;

    EnergyController energy;
    FairyInvinciblePowerUp fairy;
    TempInvincibleHelper helper; // fallback if no fairy

    void Awake()
    {
        energy = GetComponent<EnergyController>();
        fairy  = GetComponentInChildren<FairyInvinciblePowerUp>(true);
        helper = GetComponent<TempInvincibleHelper>();
        if (!helper) helper = gameObject.AddComponent<TempInvincibleHelper>();
    }

    void OnEnable()
    {
        if (energy != null)
            energy.OnDamageTaken += HandleDamageTaken;
    }

    void OnDisable()
    {
        if (energy != null)
            energy.OnDamageTaken -= HandleDamageTaken;
    }

    void HandleDamageTaken(int _)
    {
        // If the hit killed energy, your EnergyController may pause/respawn.
        // This still toggles invincibility immediately on “any” damage event.
        if (fairy)
        {
            fairy.SetTemporaryInvincibility(true);
            StartCoroutine(ClearAfter(seconds));
        }
        else
        {
            helper.Trigger(seconds);
        }
    }

    IEnumerator ClearAfter(float s)
    {
        yield return new WaitForSeconds(s);
        if (fairy && fairy.IsTemporaryOnly)
            fairy.SetTemporaryInvincibility(false);
    }
}
