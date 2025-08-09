using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public sealed class EggPickup : PickUp, IResettable
{
    [SerializeField] private float pickupDelay = 0.5f; 
    private bool _rewardGiven = false;  
    private bool _canBePickedUp = false;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _rewardGiven = false;
        _canBePickedUp = false;

        if (_collider != null)
            _collider.enabled = false;

        StartCoroutine(EnablePickupAfterDelay());
    }

    private IEnumerator EnablePickupAfterDelay()
    {
        yield return new WaitForSeconds(pickupDelay);
        _canBePickedUp = true;
        if (_collider != null)
            _collider.enabled = true;
    }

    protected override void OnPickUp(GameObject player)
    {
        if (!_canBePickedUp || _rewardGiven) return;

        RewardFactory.Instance.SpawnRandomReward(transform.position);
        _rewardGiven = true;  
        Destroy(gameObject);  
    }

    /* -------- IResettable -------- */
    public void ResetState()
    {
        _rewardGiven = false;
        _canBePickedUp = false;
        gameObject.SetActive(true);

        if (_collider != null)
            _collider.enabled = false;

        StartCoroutine(EnablePickupAfterDelay());
    }
}
