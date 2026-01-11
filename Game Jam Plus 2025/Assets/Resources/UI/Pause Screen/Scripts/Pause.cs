using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private RectTransform _visual;

    private void Start()
    {
        _visual.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            OnPause();
        }
    }

    private void OnPause()
    {
        Time.timeScale = 0f;

        _visual.gameObject.SetActive(true);
    }
    
    public void OnContinue()
    {
        Time.timeScale = 1.0f;
        _visual.gameObject.SetActive(false);
    }

    public void OnExit()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("TitleScreen");
    }
}
