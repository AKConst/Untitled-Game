using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance; //reference to itself

    public bool gameIsPaused = false; //boolean for pausing game

    [SerializeField] private Animator sceneChangeAnim; //reference to scene change animation

    private void Awake()
    {
        //if the instance is null, we set it to itself and add it onto the list of 'DontDestroyOnLoad' objects
        //which prevents this object from being destroyed when switching to a new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //if not null, we destroy the object, this ensures one copy per scene
            Destroy(gameObject); 
        }
    }

    private void Update()
    {
        //checking if the game is paused, if so, we set the timescale to 0 and 1 as appropriate
        if (gameIsPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
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
            InventoryScript.instance.InitializeInventory();
        }
        // ----------
    }

    //function that takes a scene index number as its perameter, then changes the scene to it
    public void ChangeLevel(int sceneNum)
    {
        StartCoroutine(ChangeLevelE(sceneNum));
    }

    //The actual coroutine function that changes the scene by playing an animation, waiting for it to finish
    //then switching over  to the next scene before playing the load-out animation
    private IEnumerator ChangeLevelE(int sceneNum)
    {
        sceneChangeAnim.SetTrigger("startLoad");
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadSceneAsync(sceneNum);

        sceneChangeAnim.SetTrigger("endLoad");
    }

    public void QuitGame()
    {
        Application.Quit(); //quite the game
    }
}
