using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FruitView))]
public sealed class FruitController : PickUp, IResettable
{
    [SerializeField] private FruitEffect effect;

    protected override void OnPickUp(GameObject player)
    {
        effect.Apply(player);
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
    }
}
