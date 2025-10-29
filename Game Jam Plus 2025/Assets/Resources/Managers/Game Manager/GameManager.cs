using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Other Scripts References")]
    CameraManager _cameraManager;
    QuestManager _questManager;

    void Start()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
        _questManager = FindAnyObjectByType<QuestManager>();
    }

    void Update()
    {
        
    }

    // make enemyBehavior call it when it dies
    public void UnlockNextRoom()
    {
        var currentIndex = _questManager.currentRoomIndex;
        var nextRoom = _questManager.nextRoom[currentIndex];

        _cameraManager.SetNextDirection(nextRoom);

        _questManager.UpdateRoom(); 
    }
}
