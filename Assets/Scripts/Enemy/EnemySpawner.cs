using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawner
{
    public Transform SpawnPoint;
    public int MaxEnemy = 3;
    public float SpawnInterval = 5f;
    public List<EnemySpawnData> SpawnTable;

    [HideInInspector] public float _timer;
    [HideInInspector] public int _currentEnemy;
}

[Serializable]
public class EnemySpawnData
{
    public EnemyType Type;
    [Range(0f, 100f)] public float Probability;
}