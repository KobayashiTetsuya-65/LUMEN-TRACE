using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private GameObject _normalEnemyPrefab;
    [SerializeField] private GameObject _wolfEnemyPrefab;
    [SerializeField] private GameObject _nightEnemyPrefab;

    [Header("数値設定")]
    [SerializeField] private int _poolSize = 10;

    [Header("スポーン設定")]
    [SerializeField] private List<EnemySpawner> _spawners = new List<EnemySpawner>();

    private Queue<GameObject> _normalEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> _wolfEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> _nightEnemyPool = new Queue<GameObject>();
    GameManager _gameManager;

    EnemyType _type;
    GameObject _enemy, _prefab;
    float _total;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameManager.Instance;
        for(int i =  0; i < 3; i++)
        {
            _type = (EnemyType)i;
            for(int j = 0; j < _poolSize; j++)
            {
                GameObject enemy = Instantiate(GetEnemyPrefab(_type));
                enemy.SetActive(false);
                GetObjectPool(_type).Enqueue(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IsMovie) return;

        foreach (EnemySpawner spawner in _spawners)
        {
            HandleSpawner(spawner);
        }
    }

    private void HandleSpawner(EnemySpawner spawner)
    {
        if (spawner._currentEnemy >= spawner.MaxEnemy) return;

        spawner._timer += Time.deltaTime;

        if (spawner._timer >= spawner.SpawnInterval)
        {
            SpawnEnemy(spawner);
            spawner._timer = 0f;
        }
    }
    private void SpawnEnemy(EnemySpawner spawner)
    {
        EnemyType type = GetRandomEnemyType(spawner.SpawnTable);
        GameObject prefab = GetEnemyPrefab(type);
        Queue<GameObject> pool = GetObjectPool(type);

        GameObject spawnedEnemy;

        if (pool.Count > 0)
        {
            spawnedEnemy = pool.Dequeue();
        }
        else
        {
            spawnedEnemy = Instantiate(_prefab, spawner.SpawnPoint.position, Quaternion.identity);
        }

        spawnedEnemy.transform.position = spawner.SpawnPoint.position;
        spawnedEnemy.SetActive(true);

        spawner._currentEnemy++;

        NormalEnemyController controller = spawnedEnemy.GetComponentInChildren<NormalEnemyController>();
        controller.OnDead = null;
        controller.OnDead += () => ReturnEnemy(spawnedEnemy, spawner,type);


    }
    private EnemyType GetRandomEnemyType(List<EnemySpawnData> table)
    {
        _total = 0f;
        foreach (EnemySpawnData data in table)
            _total += data.Probability;

        float rand = Random.Range(0f, _total);

        float current = 0f;
        foreach (EnemySpawnData data in table)
        {
            current += data.Probability;
            if (rand <= current)
                return data.Type;
        }

        return table[0].Type;
    }
    private void ReturnEnemy(GameObject enemy, EnemySpawner spawner, EnemyType type)
    {
        if (!enemy.activeInHierarchy) return;

        enemy.SetActive(false);
        GetObjectPool(type).Enqueue(enemy);
        spawner._currentEnemy--;
    }

    private GameObject GetEnemyPrefab(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => _normalEnemyPrefab,
            EnemyType.Wolf => _wolfEnemyPrefab,
            EnemyType.Night => _nightEnemyPrefab,
            _ => _normalEnemyPrefab
        };
    }

    private Queue<GameObject> GetObjectPool(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => _normalEnemyPool,
            EnemyType.Wolf => _wolfEnemyPool,
            EnemyType.Night => _nightEnemyPool,
            _ => _normalEnemyPool
        };
    }
}
