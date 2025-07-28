using UnityEngine;
using UnityEngine.SceneManagement;

public class deathScreenScript : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
