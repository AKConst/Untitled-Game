using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class mainMenuScript : MonoBehaviour
{
    private UIDocument _document;

    private Button _StartGameBtn, _SettingsBtn, _QuitGameBtn;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _StartGameBtn = _document.rootVisualElement.Q<Button>("StartGameBtn");
        _StartGameBtn.RegisterCallback<ClickEvent>(LoadLevel);

        _SettingsBtn = _document.rootVisualElement.Q<Button>("SettingsBtn");
        _SettingsBtn.RegisterCallback<ClickEvent>(EnterSettings);

        _QuitGameBtn = _document.rootVisualElement.Q<Button>("QuitGameBtn");
        _QuitGameBtn.RegisterCallback<ClickEvent>(QuitGame);
    }

    public void LoadLevel(ClickEvent evt)
    {
        SceneManager.LoadScene(0); //load 1st scene
    }

    public void EnterSettings(ClickEvent evt)
    {
        Debug.Log("Enter Settings");
    }

    public void QuitGame(ClickEvent evt)
    {
        Application.Quit(); //we quit the game
    }
}
