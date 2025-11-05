using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    private CameraManager _cameraManager;

    public EnemySpawn currentSpawn;
    [SerializeField] int _wavesSpawned;

    [Header("Collision Config")]
    private BoxCollider2D _collider;

    void Start()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
    }

    void Update()
    {

    }

    public void CheckWaves()
    {
        if(_wavesSpawned > currentSpawn.enemyWaves.Count)
        {
            ClearCurrentSpawns();
        }
        else
        {
            Spawn();
        }
    }
    
    public void ClearCurrentSpawns()
    {
        currentSpawn.DeactivateSpawn();

        _cameraManager.hasEnemySpawn = false;

        currentSpawn = null;
    }

    public void Spawn()
    {
        currentSpawn.SpawnEnemy();

        _wavesSpawned++;
    }
}
