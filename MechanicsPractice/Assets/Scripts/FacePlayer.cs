using UnityEditor.SearchService;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform playerPos; //variable to store the player position

    private void Start()
    {
        playerPos = GameObject.Find("PlayerCharacter").transform; //we assign value to our playerPos variable.
    }

    void Update()
    {
        //if the player position is not null, we calculate the proper rotation for the pointer to face the player
        if (playerPos != null)
        {
            Vector3 rotation = playerPos.position - transform.position;
            float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, roZ);
        }
    }
}
