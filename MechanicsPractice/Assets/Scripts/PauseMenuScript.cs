using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObj; //reference to our game manager object

    [SerializeField] private Button[] buttons; //list of buttons to configure their clicking behavior

    private void Start()
    {
        //iterating over all buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            //checking the current button index
            //based on its index, we assing an on click listener that then delegaes its functionality to the 
            //appropriate function
            switch (i)
            {
                case 0:
                    buttons[i].onClick.AddListener(delegate { onSaveClicked(); });
                    break;
                case 1:
                    buttons[i].onClick.AddListener(delegate { onLoadClicked(); });
                    break;
                case 2:
                    buttons[i].onClick.AddListener(delegate { onQuitClicked(); });
                    break;
            }
        }
    }

    private void onSaveClicked()
    {
        //we save the game
        SaveDataScript.instance.SaveGame();
    }

    private void onLoadClicked()
    {
        //we load the game
        SaveDataScript.instance.LoadGame();
    }

    private void onQuitClicked()
    {
        //we quit the game
        Application.Quit(); 
    }
}
