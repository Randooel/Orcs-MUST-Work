using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] List<EnemyBehavior> _enemyType= new List<EnemyBehavior>();

    [Header("x = enemyType / y = quantity")]
    [SerializeField] List<Vector2> spawnInfo = new List<Vector2>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
