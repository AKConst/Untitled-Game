using UnityEngine;

public class changeSceneScript : MonoBehaviour
{
    [Header("Scene Context")]
    [SerializeField] private int changeTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTag"))
        {
            GameManagerScript.instance.ChangeLevel(changeTo);
        }
    }
}
