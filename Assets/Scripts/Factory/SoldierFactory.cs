using UnityEngine;

public class SoldierFactory : ShadowEnemyFactory
{
    private GameObject _prefab;

    public SoldierFactory(GameObject prefab)
    {
        _prefab = prefab;
    }

    public override ShadowEnemy CreateShadowEnemy()
    {
        GameObject obj = Object.Instantiate(_prefab);
        return obj.GetComponent<ShadowEnemy>();
    }
}
