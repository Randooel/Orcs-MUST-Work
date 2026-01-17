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

    private void Awake()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
        _questManager = FindAnyObjectByType<QuestManager>();
        _goUI = FindAnyObjectByType<GoUI>();
    }

    void Start()
    {
        HideAndLockMouse();

        //Time.timeScale = 0.5f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Office");
        }
    }

    public void HideAndLockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowAndFreeMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
