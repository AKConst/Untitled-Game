using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int playerHealth; //value to track player HP

    //function used to check player status and execute the proper response as necessary
    public void checkPlayerStatus()
    {
        //if player HP reaches or is below 0, we move to the death screen and destroy the player object
        if (playerHealth <= 0)
        {
            Debug.Log("Player Killed!");
            enabled = false;
            GameManagerScript.instance.ChangeLevel(4);
            Destroy(gameObject);
        }
    }

    //function for updating player status, invoked to change specific values regarding player (only hp for now)
    public void updatePlayerStatus(int valKey, int amp)
    {
        switch (valKey)
        {
            case 0:
                Debug.Log("Player took " + amp + " damage!");
                playerHealth -= amp;
                break;
            default:
                Debug.Log("[-] Attempted to change improper value!");
                break;
        }
    }
}
