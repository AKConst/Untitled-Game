using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.UIElements;
public class deathScreenScript : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(0); //we load scene of the scene list at index position 0 when we click retry
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(3); //we load the main menu scene (at index 3 for now)
    }

    public void QuitGame()
    { 
        Application.Quit(); //we quit the game
    }
}
