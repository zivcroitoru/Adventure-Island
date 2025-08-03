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

        if (attacker == null)
        {
            Debug.Log("[PlayerAttack] No attacker found.");
            return;
        }

        if (!attacker.CanAttack())
        {
            Debug.Log("[PlayerAttack] Attacker can't attack right now.");
            return;
        }

        attacker.Attack(); // Plays animation and logic inside
    }
}
