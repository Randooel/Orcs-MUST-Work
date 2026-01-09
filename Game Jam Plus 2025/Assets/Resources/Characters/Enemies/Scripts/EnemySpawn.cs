using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    public int currentWaveIndex;

    [Space(10)]
    [SerializeField] List<EnemyBehavior> _enemyType = new List<EnemyBehavior>();
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();

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
        //Debug.Log("SpawnEnemy");

        var cameraManager = FindFirstObjectByType<CameraManager>();

        if (currentWaveIndex < enemyWaves.Count)
        {
            var currentWave = enemyWaves[currentWaveIndex];

            for (int i = 0; i < currentWave.spawnInfo.Count; i++)
            {
                // Selects the current enemy to be spawned, based on the currentWave spawn info
                var enemy = _enemyType[Mathf.RoundToInt(currentWave.spawnInfo[i].x)];
                var quantity = Mathf.RoundToInt(currentWave.spawnInfo[i].y);
                var spawnPoint = _spawnPoints[Mathf.RoundToInt(currentWave.spawnInfo[i].z)];


                for (int j = 0; j < quantity; j++)
                {
                    var enemyInstance = Instantiate(enemy.gameObject, spawnPoint.position, Quaternion.identity);
                    enemyInstance.GetComponent<EnemyBehavior>().wasSpawned = true;

                    cameraManager.enemyCounter++;
                }
            }

            currentWaveIndex++;
        }

        if(currentWaveIndex >= enemyWaves.Count)
        {
            cameraManager.hasEnemySpawn = false;
            gameObject.SetActive(false);
        }
    }

    public void DeactivateSpawn()
    {
        gameObject.SetActive(false);

        currentWaveIndex = 0;
    }
}

[System.Serializable]
public class EnemySpawnWave
{
    [Header("X = EnemyType Index / Y = Quantity / Z = SpawnPoint Index")]
    public List<Vector3> spawnInfo = new List<Vector3>();
}
