using UnityEngine;

public class AlienFactory : ShadowEnemyFactory
{
    private GameObject _prefab;

    public AlienFactory(GameObject prefab)
    {
        _prefab = prefab;
    }

    public override ShadowEnemy CreateShadowEnemy()
    {
        GameObject obj = Object.Instantiate(_prefab);
        return obj.GetComponent<ShadowEnemy>();
    }
}
