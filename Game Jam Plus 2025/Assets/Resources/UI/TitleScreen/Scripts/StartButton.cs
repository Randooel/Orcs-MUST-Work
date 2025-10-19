using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnButtonPressed()
    {
        SceneController.instance.NextLevel();
    }
}
