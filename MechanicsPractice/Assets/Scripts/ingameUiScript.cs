using Unity.VisualScripting;
using UnityEngine;

public class ingameUiScript : MonoBehaviour
{
    private void Update()
    {
        //toggle the pause menu on pressing 'P' as well as pause the game
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
    }
}
