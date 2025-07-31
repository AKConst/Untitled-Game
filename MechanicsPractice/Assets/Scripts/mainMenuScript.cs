using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene(0); //load 1st scene
    }

    public void QuitGame()
    {
        Application.Quit(); //we quit the game
    }
}
