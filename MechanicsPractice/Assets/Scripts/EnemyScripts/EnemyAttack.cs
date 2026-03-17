using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int enemyDamage; //value to assign amount of damage the enemy deals
    public GameObject enemyInstance; //value that will hold the enemy game object, necessary when damage type is dealt.

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)  
        {
            //if the enemy attack has collided with ours, we delete both indicators
            Debug.Log("enemy parry");
            Destroy(other.gameObject);// Destroy player's attack
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 7)  // Player layer
        {
            //If the player was hit, we deal damage and check the player status via the appropriate functions
            Debug.Log("player hit");
            other.gameObject.GetComponent<PlayerManager>().updatePlayerStatus(0, enemyDamage);
            other.gameObject.GetComponent<PlayerManager>().checkPlayerStatus();
        }
    }
}
