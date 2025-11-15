using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.UIElements;
public class deathScreenScript : MonoBehaviour
{
    private UIDocument _doc;

    private Button _RetryBtn, _GiveUpBtn;

    private void Awake()
    {
        //we get the document then assign our buttons and apply a callback event to them
        _doc = GetComponent<UIDocument>();

        _RetryBtn = _doc.rootVisualElement.Q<Button>("RessurectBtn");
        _RetryBtn.RegisterCallback<ClickEvent>(Retry);

        _GiveUpBtn = _doc.rootVisualElement.Q<Button>("GiveUpBtn");
        _GiveUpBtn.RegisterCallback<ClickEvent>(MainMenu);
    }

    public void Retry(ClickEvent evt)
    {
        SaveDataScript.instance.LoadGame();
        SceneManager.LoadScene(0); //we load scene of the scene list at index position 0 when we click retry
    }
    public void MainMenu(ClickEvent evt)
    {
        SceneManager.LoadScene(3); //we load the main menu scene (at index 3 for now)
    }
}
