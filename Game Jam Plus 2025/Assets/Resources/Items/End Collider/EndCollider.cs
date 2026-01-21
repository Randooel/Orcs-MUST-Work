using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var finalScreen = FindAnyObjectByType<GameManager>().finalScreen;
            SceneManager.LoadScene(finalScreen.Name);
        }
    }
}
