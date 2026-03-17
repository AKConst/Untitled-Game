using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int playerDamage; //value for how much damage the player attack will inflict
    [SerializeField] private int focusDamage; //value for how much focus damage the player attack will inflict

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            //on a successful parry, we get the instance of the enemy game object that spawned the attack
            //that is referenced in the enemy script, and from there we run the functions to deal stagger damage
            //and then to update the enemy state if appropriate
            Debug.Log("Player parry");
            GameObject enemyOfAttack = other.gameObject.GetComponent<EnemyAttack>().enemyInstance;
            enemyOfAttack.GetComponent<EnemyGeneric>().updateEnemyStatus(1, 1);
            enemyOfAttack.GetComponent<EnemyGeneric>().checkEnemyStatus();

            //we deletete the attack indicators for the enemy and ourselves
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == 6)
        {
            //if we have hit an enemy object cirectly, then we deal direct and focus damage as well as run the
            //functions to check on the enemy status
            Debug.Log("enemy hit");
            other.gameObject.GetComponent<EnemyGeneric>().updateEnemyStatus(0, playerDamage);
            other.gameObject.GetComponent<EnemyGeneric>().updateEnemyStatus(2, focusDamage);
            other.gameObject.GetComponent<EnemyGeneric>().checkEnemyStatus();
        }
    }
}
