using UnityEditor.SearchService;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform playerPos;

    private void Start()
    {
        playerPos = GameObject.Find("PlayerCharacter").transform;
    }

    void Update()
    {
        if (playerPos != null)
        {
            Vector3 rotation = playerPos.position - transform.position;
            float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, roZ);
        }
    }
}
