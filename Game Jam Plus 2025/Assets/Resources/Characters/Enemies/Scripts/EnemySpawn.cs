using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] List<EnemyBehavior> _enemyType = new List<EnemyBehavior>();
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();

    [Space(10)]
    [SerializeField] int _currentWaveIndex;

    [Space(10)]
    public List<EnemySpawnWave> enemyWaves = new List<EnemySpawnWave>();

    private void Start()
    {
        if(_spawnPoints.Count != transform.childCount)
        {
            _spawnPoints.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                _spawnPoints.Add(transform.GetChild(i).transform);
            }
        }
        

        //SpawnEnemy();        
    }

    public void SpawnEnemy()
    {
        var currentWave = enemyWaves[_currentWaveIndex];

        for (int i = 0; i < currentWave.spawnInfo.Count; i++)
        {
            // Selects the current enemy to be spawned, based on the currentWave spawn info
            var enemy = _enemyType[Mathf.RoundToInt(currentWave.spawnInfo[i].x)];
            var quantity = Mathf.RoundToInt(currentWave.spawnInfo[i].y);
            var spawnPoint = _spawnPoints[Mathf.RoundToInt(currentWave.spawnInfo[i].z)];

            for (int j = 0; j < quantity; j++)
            {
                Instantiate(enemy.gameObject, spawnPoint.position, Quaternion.identity);
            }
            

            // Todo: Apply quantity (spawnInfo.y) / Determine spawn position
        }

        _currentWaveIndex++;
    }

    public void DeactivateSpawn()
    {
        gameObject.SetActive(false);

        _currentWaveIndex = 0;
    }
}

[System.Serializable]
public class EnemySpawnWave
{
    [Header("<b>X</b> = EnemyType Index / <b>Y</b> = Quantity / <b>Z</b> = SpawnPoint Index")]
    public List<Vector3> spawnInfo = new List<Vector3>();
}
