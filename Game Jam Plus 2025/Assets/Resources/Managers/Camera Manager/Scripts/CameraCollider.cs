using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private BorderCollision borderCollision;

    private void Start()
    {
        borderCollision = GetComponentInParent<BorderCollision>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            borderCollision.ResetColliders();

            borderCollision.ActivateNavigation();
        }
    }
}
