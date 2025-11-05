using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private CameraManager _cameraManager;
    private BorderCollision borderCollision;

    [SerializeField] bool isRay;
    [SerializeField] BoxCollider2D _collider;

    private void Start()
    {
        borderCollision = GetComponentInParent<BorderCollision>(); 
        _cameraManager = FindAnyObjectByType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRay)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<EnemyBehavior>();

                if(enemy.CurrentState != EnemyBehavior.State.Death)
                {
                    _cameraManager.enemyCounter++;
                }
            }

            if(collision.CompareTag("EnemySpawn"))
            {
                Debug.Log("Colidiu com " + collision.name);

                // Add enemySpawn elements to the currentSpawns list
                var enemySpawn = collision.GetComponent<EnemySpawn>();
                var enemySpawnManager = FindAnyObjectByType<EnemySpawnManager>();

                enemySpawnManager.currentSpawns.Add(enemySpawn);

                _cameraManager.hasEnemySpawn = true;
            }
            else
            {
                _cameraManager.hasEnemySpawn = false;

                //Clear currentSpawnList
                var enemySpawnManager = FindAnyObjectByType<EnemySpawnManager>();
                enemySpawnManager.ClearCurrentSpawns();
            }

            _collider.enabled = false;
        }
        else if (collision.CompareTag("Player"))
        {
            borderCollision.ResetColliders();

            borderCollision.ActivateNavigation();
        }
        
    }
}
