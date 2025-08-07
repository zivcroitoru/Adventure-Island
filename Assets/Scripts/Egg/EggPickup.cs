using UnityEngine;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp, IResettable
{
    private bool _rewardGiven = false;  // Flag to track if reward has been given

    /* -------- Public API -------- */
    private void Start()
    {
        if (RewardFactory.Instance == null)
        {
            Debug.LogError("RewardFactory instance is not set. Ensure RewardFactory is initialized before EggPickup.");
            return;
        }
    }

    protected override void OnPickUp(GameObject player)
    {
        // Prevent giving multiple rewards for the same egg
        if (_rewardGiven) return;

        if (RewardFactory.Instance == null)
        {
            Debug.LogError("RewardFactory instance is still null when trying to pick up.");
            return;
        }

        Debug.Log($"Egg picked up by {player.name}. Spawning reward at {transform.position}");
        RewardFactory.Instance.SpawnRandomReward(transform.position);

        _rewardGiven = true;  // Mark the reward as given
        Destroy(gameObject);  // Destroy the egg after it's been picked up
    }

    /* -------- IResettable -------- */
    public void ResetState()
    {
        // If you need to reset the egg (for example, in a pool system),
        // we can reactivate it and reset the reward state flag.
        _rewardGiven = false;
        gameObject.SetActive(true);
    }
}
