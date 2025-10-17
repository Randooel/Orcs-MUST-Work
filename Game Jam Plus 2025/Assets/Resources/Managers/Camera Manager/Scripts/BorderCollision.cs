using UnityEngine;

public class BorderCollision : MonoBehaviour
{
    [Header("Other Scripts References")]
    CameraManager _cameraManager;

    [Header("Collision Config")]
    public Collider2D[] colliders; // [0]: up / [1]: left / [2]: right / [3]: down
    public int currentTriggerCollider;

    private void Start()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
        colliders = GetComponents<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ResetColliders();

            _cameraManager.NavigateTo(_cameraManager.NextDirection);
        }
    }

    public void ResetColliders()
    {
        foreach (var c in colliders)
        {
            c.isTrigger = false;
        }
    }
}
