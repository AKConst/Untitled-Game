using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Android.Gradle;
using System;

public class EnemyGeneric : MonoBehaviour
{
    //Enemy attack settings
    [Header("Attack Settings")]
    [SerializeField] private GameObject attackIndicator; //reference to our attack indicator
    [SerializeField] private float timeToAttack; //how long will the enemy charge up its attack
    [SerializeField] private float atkHitRange; //how big is the range of the attack
    [SerializeField] private LayerMask whatIsPlayer; //layermask to know whether or not something is the player
    [SerializeField] private Transform attackPos; //position where the attack will spawn
    [SerializeField] private GameObject attackSprite; //sprite for the attack


    //booleans for keeping track of attack status
    private bool canAttack = true; 
    private bool isAttacking = false;
    [SerializeField] private GameObject AttackCircle;

    //different Collider2D areas to check various conditions to change enemy state
    private Collider2D playerLeaveArea; //ended up being unnecessary, will leave here if useful at any point
    private Collider2D playerDetectArea;
    private Collider2D playerAttackArea;

    //detection settings for the enemy
    [Header("Detection Settings")]
    [SerializeField] private float DetectRange; //range for the area that the enemy can detect the player in
    [SerializeField] private float LeavePlayerRange; //range for the area for the enemy to leave the player alone
    [SerializeField] private float AttackRange; //range for the enemy to start its attack

    //lists of the chase scripts that will be used to enable/disable the enemy chasing when attacking or leaving the player
    [SerializeField] private MonoBehaviour[] chaseScripts;
    //script for aligning its attack pointer to the player
    private MonoBehaviour atkAlignScript; 

    //enum of the various enemy states, as well as a variable to store the current state
    private enum enemyState { enemyChase, enemyIdle, enemyAttack }; 
    private enemyState currState;

    void Start()
    {
        atkAlignScript = this.GetComponentInChildren<FacePlayer>(); //assigning value to the attack indicator align script
        //iterating through all chase scripts and disabling them, making the enemies main state to be idle
        foreach (MonoBehaviour item in chaseScripts)
        {
            item.enabled = false;
        }
    }

    void Update()
    {
        updateState(); //running a function to update the enemy state

        //based on the current state we run the appropriate function
        switch (currState)
        {
            case enemyState.enemyChase:
                //when chasing the player, we go through the chase scripts list and enable them all
                foreach (MonoBehaviour item in chaseScripts)
                {
                    item.enabled = true;
                }
                break;
            case enemyState.enemyAttack:
                //when attacking, we check the appropriate conditions for attacking and if met
                //we disable the chase scripts then proceed to run the attack coroutine
                if (canAttack)
                {
                    foreach (MonoBehaviour item in chaseScripts)
                    {
                        item.enabled = false;
                    }
                    StartCoroutine(Attack());
                }
                break;
            case enemyState.enemyIdle:
                //when idling, we just disable the chase scripts yet again
                foreach (MonoBehaviour item in chaseScripts)
                {
                    item.enabled = false;
                }
                break;
        }
    }

    private void updateState()
    {
        //we draw 3 different areas, each for checking conditions for different states
        playerLeaveArea = Physics2D.OverlapCircle(transform.position, LeavePlayerRange, whatIsPlayer);
        playerDetectArea = Physics2D.OverlapCircle(transform.position, DetectRange, whatIsPlayer);
        playerAttackArea = Physics2D.OverlapCircle(transform.position, AttackRange, whatIsPlayer);
        //checking the areas and changing the enemy state as appropriate
        if (playerDetectArea != null && playerAttackArea == null && !isAttacking)
        {
            currState = enemyState.enemyChase;
        }
        else if (playerAttackArea != null && canAttack)
        {
            currState = enemyState.enemyAttack;
        }
        else
        {
            currState = enemyState.enemyIdle;
        }
    }

    private IEnumerator Attack()
    {
        //setting our booleans for the attacking status
        canAttack = false;
        isAttacking = true;

        //since the attack has begun, we dont want the enemy facing indicator to move to a new position
        //so we disable the script that changes its alignment
        atkAlignScript.enabled = false;

        //we instantiate the enemy attack indicator above the enemy and parent it to the enemys transform
        GameObject eAtkIndicatorInstance = Instantiate(attackIndicator, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        eAtkIndicatorInstance.transform.parent = transform;
        //we wait for the attack chargeup time to finish before we delete the indicator and proceed with the attack
        yield return new WaitForSeconds(timeToAttack);
        Destroy(eAtkIndicatorInstance);

        //we instantiate the sprite for the enemy attack
        //GameObject eAtkHitAreaInstance = Instantiate(attackSprite, attackPos.position, Quaternion.identity);
        //after waiting 0.1 seconds, we destroy the attack indicator
        //yield return new WaitForSeconds(0.1f);
        //Destroy(eAtkHitAreaInstance);

        //we create an overlap circle that represents the attack hit area, and if the player was hit
        //we destroy the player game object, disable this script and change the level to the death screen
        //which is currently at index 4.
        //Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos.position, atkHitRange, whatIsPlayer);
        
        GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.identity);
        StartCoroutine(deleteIndicator(Attack));
        
        /*if (hitPlayer != null)
        {
            PlayerAttack player = hitPlayer.GetComponentInChildren<PlayerAttack>(); // finds the PlayerAttack script to check if player is blocking
            if (player == null)
            {
                Debug.Log("player null"); //error player is not found
            }
            else if(!player.isAttacking) // Player is not attacking
            {
                Destroy(hitPlayer.gameObject);
                this.enabled = false;
                GameManagerScript.instance.ChangeLevel(4);
            }
            else if (player.isAttacking)// Player is attacking
            {
                Debug.Log("Attack was blocked!");
            }
        }*/

        //setting our attack status booleans
        canAttack = true;
        isAttacking = false;

        //re-enabling the enemy facing direction indicator
        atkAlignScript.enabled = true;
    }

    //function to draw on screen gizmos to better visualize the enemy state areas
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LeavePlayerRange);
    }

    private IEnumerator deleteIndicator(GameObject ind)
    {
        //wait for 0.1 seconds before deleting the attack sprite
        yield return new WaitForSeconds(0.3f);
        Destroy(ind);
    }
}
