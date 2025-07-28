using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public static bool gameIsPaused = false;

    [SerializeField] private Animator sceneChangeAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void ChangeLevel(int sceneNum)
    {
        StartCoroutine(ChangeLevelE(sceneNum));
    }

    private IEnumerator ChangeLevelE(int sceneNum)
    {
        sceneChangeAnim.SetTrigger("startLoad");
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadSceneAsync(sceneNum);

        sceneChangeAnim.SetTrigger("endLoad");
    }
}
