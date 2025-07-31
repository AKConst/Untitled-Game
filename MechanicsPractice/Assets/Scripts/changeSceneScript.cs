using UnityEngine;

public class changeSceneScript : MonoBehaviour
{
    //allowing us to set the index value of the scene that this scene change trigger will change to
    [Header("Scene Context")]
    [SerializeField] private int changeTo;

    //check if the player is the one that has entered the trigger collider and if so we call the
    //change level function from the game manager instance and pass the index of the scene to it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTag"))
        {
            GameManagerScript.instance.ChangeLevel(changeTo);
        }
    }
}
