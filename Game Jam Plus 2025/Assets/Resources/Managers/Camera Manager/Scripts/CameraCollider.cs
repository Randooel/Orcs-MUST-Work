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
            //Debug.Log(collision.name);

            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<EnemyBehavior>();

                if (enemy.CurrentState != EnemyBehavior.State.Death)
                {
                    _cameraManager.enemyCounter++;
                }
            }
            else if (collision.CompareTag("EnemySpawn"))
            {
                //Debug.Log("Colidiu com " + collision.name);

                // Add enemySpawn elements to the currentSpawns list
                var enemySpawn = collision.GetComponent<EnemySpawn>();

                _cameraManager.hasEnemySpawn = true;
                _cameraManager.currentSpawn = enemySpawn;
            }
            if (collision.CompareTag("NPC"))
            {
                _cameraManager.hasNPC = true;
                _cameraManager.currentNPC = collision.GetComponent<NPCBehavior>();
            }

            //_collider.enabled = false;
        }
        else if (collision.CompareTag("Player"))
        {
            borderCollision.ResetColliders();

            borderCollision.ActivateNavigation();
        }

        if(isRay)
        {
            _collider.enabled = false;
        }
    }
}
