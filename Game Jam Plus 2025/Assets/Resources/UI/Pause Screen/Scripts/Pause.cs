using Sirenix.OdinInspector;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameManager _gameManager;

    [PropertySpace(SpaceAfter = 10)]
    [SerializeField] private bool isPaused;

    [SerializeField] private InputAction togglePause;

    [SerializeField] private RectTransform _visual;

    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    #region Input System Related
    private void OnEnable()
    {
        togglePause.Enable();
        togglePause.performed += TogglePause;
    }

    private void OnDisable()
    {
        togglePause.Disable();
        togglePause.performed -= TogglePause;
    }
    #endregion

    private void Start()
    {
        _visual.gameObject.SetActive(false);
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            OnPause();
        }
        else
        {
            OnContinue();
        }
    }

    private void OnPause()
    {
        isPaused = true;

        _gameManager.ShowAndFreeMouse();

        Time.timeScale = 0f;
        _visual.gameObject.SetActive(true);
    }
    
    public void OnContinue()
    {
        isPaused = false;

        _gameManager.HideAndLockMouse();

        Time.timeScale = 1.0f;
        _visual.gameObject.SetActive(false);
    }

    public void OnExit()
    {
        //isPaused = false;

        var titleScene = _gameManager.titleScreen;

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(titleScene.Name);
    }
}
