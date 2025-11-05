using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] List<EnemyBehavior> _enemyType = new List<EnemyBehavior>();

    [Space(10)]
    [SerializeField] int _currentWaveIndex;

    [Space(10)]
    [SerializeField] List<EnemySpawnWave> _enemyWaves = new List<EnemySpawnWave>();

    private void Start()
    {
        //SpawnEnemy();        
    }

    public void SpawnEnemy()
    {
        var currentWave = _enemyWaves[_currentWaveIndex];
        for (int i = 0; i < currentWave.spawnInfo.Count; i++)
        {
            // Selects the current enemy to be spawned, based on the currentWave spawn info
            var currentEnemy = _enemyType[Mathf.RoundToInt(currentWave.spawnInfo[i].x)];

            Instantiate(currentEnemy.gameObject);

            // Todo: Apply quantity (spawnInfo.y) / Determine spawn position
        }
    }

    public void DeactivateSpaw()
    {

    }
}

[System.Serializable]
public class EnemySpawnWave
{
    [Header("x = enemyType / y = quantity")]
    public List<Vector2> spawnInfo = new List<Vector2>();
}
