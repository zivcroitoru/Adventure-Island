using UnityEngine;

[DisallowMultipleComponent]
public class AxeProjectilePool : MonoBehaviour
{
    [SerializeField] private ObjectPool<ProjectileAxe> _pool;

    public ProjectileAxe Spawn(Vector3 pos, Quaternion rot)
        => _pool.Spawn(pos, rot);

    public ObjectPool<ProjectileAxe> Pool => _pool;
}
