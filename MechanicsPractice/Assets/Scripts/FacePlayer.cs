using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = playerPos.position - transform.position;
        float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, roZ);
    }
}
