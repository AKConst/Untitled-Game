using UnityEngine;

public class ingameUiScript : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenuUI;

    private void Update()
    {
        //toggle the iventory on pressing 'I'
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
            }
            else { 
                inventoryUI.SetActive(true);
            }
        }

        //toggle the pause menu on pressing 'P' as well as pause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuUI.activeSelf)
            {
                GameManagerScript.instance.gameIsPaused = false;
                pauseMenuUI.SetActive(false);
            }
            else
            {
                GameManagerScript.instance.gameIsPaused = true;
                pauseMenuUI.SetActive(true);
            }
        }
    }
}
