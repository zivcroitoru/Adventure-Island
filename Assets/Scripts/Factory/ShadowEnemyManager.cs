using UnityEngine;

public class ShadowEnemyManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject alienPrefab;

    private SoldierFactory _soldierFactory;
    private AlienFactory _alienFactory;

    void Awake()
    {
        _soldierFactory = new SoldierFactory(soldierPrefab);
        _alienFactory = new AlienFactory(alienPrefab);
    }

private void Start()
{
    SpawnEnemy(_soldierFactory, new Vector3(-2, -1.5f, 0)); // left and slightly below player
    SpawnEnemy(_alienFactory, new Vector3(2, -1.5f, 0));    // right and slightly below player
}


    private void Update()
    {
        // Q = new soldier to the left of player
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 offset = new Vector3(Random.Range(-4f, -2f), Random.Range(-2f, -1f), 0);
            SpawnEnemy(_soldierFactory, offset);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 offset = new Vector3(Random.Range(2f, 4f), Random.Range(-2f, -1f), 0);
            SpawnEnemy(_alienFactory, offset);
        }

    }

    private ShadowEnemy SpawnEnemy(ShadowEnemyFactory factory, Vector3 offset)
    {
        ShadowEnemy enemy = factory.CreateShadowEnemy();
        enemy.transform.position = playerTransform.position + offset;
        return enemy;
    }
}
