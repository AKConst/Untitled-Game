using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyGeneric : MonoBehaviour
{
    //Enemy attack settings
    [Header("Enemy Settings")]
    [SerializeField] private int enemyHealth; //value to keep track of enemy HP
    //value to keep track when enemy will be staggered after a successful parry
    [SerializeField] private int enemyStaggerCount;
    private int enemyStaggerCountLive; //the value that we update and check. Will be reset to original value once <=0
    //value to keep track of how many times the enemy will be able to finish an attack if interrupted by player attack
    [SerializeField] private int enemyFocusCount;
    private int enemyFocusCountLive; //the value that we update and check. Will be reset to original value once <=0
    private bool isStaggered = false; //to keep staggered status
    [SerializeField] private float focusRegainTime; //value to assign the value of how much time the enemy will refocus

    //booleans for keeping track of attack status
    public bool canAttack = true; 
    public bool isAttacking = false;

    //lists of the chase scripts that will be used to enable/disable the enemy chasing when attacking or leaving the player
    [Header("Behavior Scripts")]
    [SerializeField] private MonoBehaviour chaseScript;
    [SerializeField] private MonoBehaviour idleScript;
    [SerializeField] private MonoBehaviour attackScript;
    //script for aligning its attack pointer to the player
    private MonoBehaviour atkAlignScript; 

    //enum of the various enemy states, as well as a variable to store the current state
    public enum enemyState { enemyChase, enemyIdle, enemyAttack, enemyDamaged, enemyStaggered }; 
    public enemyState currState;

    void Start()
    {
        atkAlignScript = GetComponentInChildren<FacePlayer>(); //assigning value to the attack indicator align script
        //iterating through all chase scripts and disabling them, making the enemies main state to be idle
        chaseScript.enabled = false;

        //assigning the initial values for the variables that will vary/change
        enemyStaggerCountLive = enemyStaggerCount;
        enemyFocusCountLive = enemyFocusCount;
    }

    void Update()
    {
        //we turn the attack align script depending on the state of the enemy attacking
        if (isAttacking)
        {
            atkAlignScript.enabled = false;
        }
        else
        {
            atkAlignScript.enabled = true;
        }

        //based on the current state we run the appropriate function
        switch (currState)
        {
            case enemyState.enemyChase:
                //when chasing the player, we disable other scripts and enable the chase script
                idleScript.enabled = false;
                atkAlignScript.enabled = true;

                chaseScript.enabled = true;
                break;
            case enemyState.enemyAttack:
                //when attacking, we check the appropriate conditions for attacking and if met
                //we disable the chase scripts then proceed to run the attack coroutine
                if (canAttack)
                {
                    idleScript.enabled = false;
                    chaseScript.enabled = false;

                    attackScript.GetComponent<EnemyAttackStateGeneric>().StartAttack();
                }
                break;
            case enemyState.enemyIdle:
                //when idling, we just disable the chase scripts yet 
                chaseScript.enabled = false;
                attackScript.enabled = false;

                idleScript.enabled = true;
                break;
            //we essentially put the functionality as if the enemy were idling, needs better implementation for
            //putting the enemy in an inactive state cause this is too much code reuse
            case enemyState.enemyDamaged:
                idleScript.enabled = false;
                chaseScript.enabled = false;
                break;
            case enemyState.enemyStaggered:
                idleScript.enabled = false;
                chaseScript.enabled = false;
                break;
        }
    }

    private IEnumerator staggerEnemy(float staggerTime)
    {
        //we put the enemy in a staggered state-wait X amt of time-reset the state and stagger counter.
        currState = enemyState.enemyStaggered;
        yield return new WaitForSeconds(staggerTime);
        enemyStaggerCountLive = enemyStaggerCount;
        currState = enemyState.enemyIdle;
        Debug.Log("Enemy Exited Staggered State!");
    }

    private IEnumerator resetFocus()
    {
        //we put the enemy in a damaged state-wait X amt of time-reset the state and focus counter.
        currState = enemyState.enemyDamaged;
        yield return new WaitForSeconds(focusRegainTime);
        enemyFocusCountLive = enemyFocusCount;
        currState = enemyState.enemyIdle;
    }

    //function to check enemy status and run the proper response to the status.
    public void checkEnemyStatus()
    {
        if (enemyHealth <= 0) {
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
            return; 
        }
        else if (enemyStaggerCountLive <= 0) {
            Debug.Log("Stagger Enemy!");
            StartCoroutine(staggerEnemy(1f));
        }
        else if (enemyFocusCountLive <= 0)
        {
            Debug.Log("Enemy attack can be interrupted!");
            StartCoroutine(resetFocus());
        }
    }

    //function to update enemy status, to change various values regarding the enemy.
    public void updateEnemyStatus(int valValue, int amp)
    {
        switch (valValue)
        {
            case 0:
                Debug.Log("Enemy hit for " + amp + " damage!");
                enemyHealth -= amp;
                break;
            case 1:
                Debug.Log("Enemy staggered for " + amp + " damage!");
                enemyStaggerCountLive -= amp;
                break;
            case 2: 
                enemyFocusCountLive -= amp;
                break;
            default:
                Debug.Log("Enemy focus deprecated by " + amp + " damage!");
                Debug.Log("[-] Value Out Of Scope!");
                break;
        }
    }

    public void SetCurrState(enemyState ns)
    {
        currState = ns;
    }
}
