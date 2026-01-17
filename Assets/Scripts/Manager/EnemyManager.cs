using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("éQè∆")]
    [SerializeField] private GameObject _normalEnemy;

    [Header("êîílê›íË")]
    [SerializeField] private int _poolSize = 10;

    private Queue<GameObject> _enemyPool = new Queue<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i  = 0; i < _poolSize; i++)
        {
            GameObject enemy = Instantiate(_normalEnemy);
            _enemyPool.Enqueue(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpownEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                if(_enemyPool.TryDequeue(out GameObject enemy))
                {

                }
                else
                {

                }
                    break;
        }
    }
}
