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
        if(colliders == null)
        {
            Debug.Log("No camera colliders found!");
        }
    }

    public void ActivateNavigation()
    {
        _cameraManager.NavigateTo(_cameraManager.NextDirection);
    }

    public void ResetColliders()
    {
        foreach (var c in colliders)
        {
            c.isTrigger = false;
        }
    }
}
