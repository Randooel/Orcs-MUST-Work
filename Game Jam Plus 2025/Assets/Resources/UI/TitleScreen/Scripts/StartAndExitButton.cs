using UnityEngine;

public class StartAndExitButton : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        SceneController.instance.NextLevel();
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
