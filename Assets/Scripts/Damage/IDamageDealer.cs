using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    // Method to deal damage to a target
    void DealDamage(GameObject target);

    // Method to calculate the amount of damage to deal based on the target
    int CalculateDamage(GameObject target);

    // Optionally, a flag to indicate whether the damage dealer should be destroyed after hitting the target
    bool DestroyAfterHit { get; set; }
}
