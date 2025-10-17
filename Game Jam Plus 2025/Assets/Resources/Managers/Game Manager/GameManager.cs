using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Other Scripts References")]
    CameraManager _cameraManager;

    void Start()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
    }

    void Update()
    {
        
    }
}
