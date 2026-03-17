using UnityEngine;

public class CollisionDetectionShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            Debug.Log("Shield Destroyed");
            //we deletete the attack indicators for the enemy and the shield
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
