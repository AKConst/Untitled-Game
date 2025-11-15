using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    private UIDocument doc;
    private Button resumeBtn, settingsBtn, mainMenuBtn, quitBtn;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();

        resumeBtn = doc.rootVisualElement.Q<Button>("ResumeBtn");
        settingsBtn = doc.rootVisualElement.Q<Button>("SettingsBtn");
        mainMenuBtn = doc.rootVisualElement.Q<Button>("MainMenuBtn");
        quitBtn = doc.rootVisualElement.Q<Button>("QuitBtn");

        resumeBtn.RegisterCallback<ClickEvent>(resumeGame);
        settingsBtn.RegisterCallback<ClickEvent>(enterSettings);
        mainMenuBtn.RegisterCallback<ClickEvent>(enterMainMenu);
        quitBtn.RegisterCallback<ClickEvent>(quitGame);
    }


    private void OnEnable()
    {
        resumeBtn = doc.rootVisualElement.Q<Button>("ResumeBtn");
        settingsBtn = doc.rootVisualElement.Q<Button>("SettingsBtn");
        mainMenuBtn = doc.rootVisualElement.Q<Button>("MainMenuBtn");
        quitBtn = doc.rootVisualElement.Q<Button>("QuitBtn");

        resumeBtn.RegisterCallback<ClickEvent>(resumeGame);
        settingsBtn.RegisterCallback<ClickEvent>(enterSettings);
        mainMenuBtn.RegisterCallback<ClickEvent>(enterMainMenu);
        quitBtn.RegisterCallback<ClickEvent>(quitGame);
    }

    private void resumeGame(ClickEvent evt)
    {
        //we unpause the game
        Debug.Log("Resume game!");
        GameManagerScript.instance.UnpauseGameFunc();
    }

    private void enterSettings(ClickEvent evt)
    {
        Debug.Log("Enter Settings!");
    }

    private void enterMainMenu(ClickEvent evt)
    {
        //we load into the main menu
        Debug.Log("Go to main menu!");
        GameManagerScript.instance.UnpauseGameFunc();
        SceneManager.LoadScene(3);
    }

    private void quitGame(ClickEvent evt)
    {
        //we quit the game
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
