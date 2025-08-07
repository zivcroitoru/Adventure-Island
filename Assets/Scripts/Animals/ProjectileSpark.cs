using UnityEngine;
public sealed class ProjectileSpark : BaseProjectile
{
    [SerializeField] private float lifetime = 0.3f;
    [SerializeField] private float flickerInterval = 0.05f;

    private bool _flipState;

    public override void Shoot(Vector2 origin, Vector2 direction, float playerSpeed = 0f)
    {
        transform.position = origin;
        transform.rotation = Quaternion.identity;

        rb.velocity = direction.normalized * _speed;
        _flipState = direction.x < 0;
        if (sr != null) sr.flipX = _flipState;

        InvokeRepeating(nameof(ToggleFlip), 0f, flickerInterval);
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void ToggleFlip()
    {
        _flipState = !_flipState;
        if (sr != null) sr.flipX = _flipState;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        CancelInvoke(nameof(ToggleFlip));
        _flipState = false;
    }
}
