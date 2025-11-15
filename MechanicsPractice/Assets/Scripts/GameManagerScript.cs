using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance; //reference to itself
    public static bool gameIsPaused = false; //boolean for pausing game

    //references to the main game UI and the pause menu UI (settings menu will probably have to be added in the future)
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenuUI;

    //reference to all the scripts that need to be enabled/disabled when pausing the game
    [SerializeField] private MonoBehaviour[] scriptsToPause; 

    private void Awake()
    {
        //if the instance is null, we set it to itself
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameManagerScript.gameIsPaused)
            {
                GameManagerScript.instance.UnpauseGameFunc();
            }
            else
            {
                GameManagerScript.instance.PauseGameFunc();
            }
        }

        //This is here to test game save mechanics, you can use the keyboard inputs here if you want to
        // remove after proper implementation
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Saved Game!");
            SaveDataScript.instance.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loaded Game!");
            SaveDataScript.instance.LoadGame();
        }
        // ----------
    }

    public void PauseGameFunc()
    {
        Debug.Log("Pausing Game!");

        gameIsPaused = true;
        inventoryUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        foreach (MonoBehaviour script in scriptsToPause)
        {
            script.enabled = false;
        }
    }
    public void UnpauseGameFunc()
    {
        Debug.Log("Unpausing Game!");

        gameIsPaused = false;
        inventoryUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        foreach (MonoBehaviour script in scriptsToPause)
        {
            script.enabled = true;
        }
    }

    //function that takes a scene index number as its perameter, then changes the scene to it
    public void ChangeLevel(int sceneNum)
    {
        SceneManager.LoadSceneAsync(sceneNum);
    }

    public void QuitGame()
    {
        Application.Quit(); //quite the game
    }
}
