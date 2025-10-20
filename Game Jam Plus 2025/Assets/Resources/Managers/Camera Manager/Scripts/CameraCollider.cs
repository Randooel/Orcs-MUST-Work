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
        
        if(isRay)
        {
            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRay)
        {
            if (collision.CompareTag("Enemy"))
            {
                _cameraManager.enemyCounter++;
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
