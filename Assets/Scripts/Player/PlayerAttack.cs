using UnityEngine;

[RequireComponent(typeof(RideController))]
public class PlayerAttack : MonoBehaviour
{
    private RideController rideController;
    private WeaponsHandler weaponsHandler;

    private void Awake()
    {
        rideController = GetComponent<RideController>();
        weaponsHandler = GetComponentInChildren<WeaponsHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            TryAttack();
    }

    private void TryAttack()
    {
        IAttacker attacker = rideController?.CurrentAttacker ?? weaponsHandler;

        Debug.Log($"[PlayerAttack] rideAttacker={(rideController?.CurrentAttacker != null)}, " +
                  $"weaponsHandler={(weaponsHandler != null)}, " +
                  $"chosen={(attacker != null ? attacker.GetType().Name : "null")}");

        if (attacker == null)
        {
            Debug.Log("[PlayerAttack] ❌ No attacker found.");
            return;
        }

        bool canAttack = attacker.CanAttack();
        Debug.Log($"[PlayerAttack] Attacker={attacker.GetType().Name}, CanAttack={canAttack}");

        if (!canAttack)
        {
            // Try to get more detail if attacker is a MonoBehaviour
            if (attacker is MonoBehaviour mb)
                Debug.Log($"[PlayerAttack] Attacker object: {mb.name} active={mb.isActiveAndEnabled}");

            Debug.Log("[PlayerAttack] ❌ Attacker can't attack right now.");
            return;
        }

        Debug.Log("[PlayerAttack] ✅ Attacking now!");
        attacker.Attack(); // Plays animation and logic inside
    }
}
