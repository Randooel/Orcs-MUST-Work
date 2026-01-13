using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    [Header("Other Scripts References")]
    CameraManager _cameraManager;
    QuestManager _questManager;
    GoUI _goUI;


    void Start()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
        _questManager = FindAnyObjectByType<QuestManager>();

        _goUI = FindAnyObjectByType<GoUI>();

        HideAndLockMouse();

        //Time.timeScale = 0.5f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Office");
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void HideAndLockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // make enemyBehavior call it when it dies
    public void UnlockNextRoom()
    {
        var currentIndex = _questManager.currentRoomIndex;
        var nextRoom = _questManager.nextRoom[currentIndex];

        _cameraManager.SetNextDirection(nextRoom);

        _questManager.UpdateRoom();

        _goUI.SetDirection();
    }
}
