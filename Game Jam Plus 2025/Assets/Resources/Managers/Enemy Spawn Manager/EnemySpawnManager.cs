using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public List<EnemySpawn> currentSpawns = new List<EnemySpawn>();

    [Header("Collision Config")]
    private BoxCollider2D _collider;

    void Start()
    {
        
    }

    void Update()
    {

    }

    // currentSpawn elements are added by the CameraCollider collision detection method
    public void ClearCurrentSpawns()
    {
        currentSpawns.Clear();
    }

    public void Spawn()
    {
        foreach(var spawn in currentSpawns)
        {
            spawn.SpawnEnemy();
        }
    }
}
