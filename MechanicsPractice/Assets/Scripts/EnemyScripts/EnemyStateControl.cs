using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static EnemyGeneric;
using System;

public class EnemyStateControl : MonoBehaviour
{
    [Header("Enemy State Variables:")]
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] Transform target;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectRange;
    [SerializeField] private float leavePlayerRange;

    private enemyState currState;

    private Collider2D playerLeaveArea;
    private Collider2D playerDetectArea;
    private Collider2D playerAttackArea;

    private Vector3Int currPos;
    private Vector3Int targetPos;

    private void Start()
    {
        currState = enemyState.enemyIdle;
    }

    private void Update()
    {
        currPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        targetPos = new Vector3Int(Mathf.FloorToInt(target.position.x), Mathf.FloorToInt(target.position.y), Mathf.FloorToInt(target.position.z));

        UpdateState();

        GetComponent<EnemyGeneric>().SetCurrState(currState);
    }

    private void UpdateState()
    {
        //if the enemy is in a damaged or staggered state, we don't update our further
        if (currState == enemyState.enemyDamaged || currState == enemyState.enemyStaggered) { return; }

        //we draw 3 different areas, each for checking conditions for different states
        playerLeaveArea = Physics2D.OverlapCircle(transform.position, leavePlayerRange, whatIsPlayer);
        playerDetectArea = Physics2D.OverlapCircle(transform.position, detectRange, whatIsPlayer);
        playerAttackArea = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

        //checking the areas and changing the enemy state as appropriate
        if (playerDetectArea != null && playerAttackArea == null)
        {
            if (TileGridData.instance.HasLOS(currPos, targetPos) && !GetComponent<EnemyGeneric>().isAttacking)
            {
                currState = enemyState.enemyChase;
            }
        }
        else if (playerAttackArea != null)
        {
            if (TileGridData.instance.HasLOS(currPos, targetPos) && GetComponent<EnemyGeneric>().canAttack)
            {
                currState = enemyState.enemyAttack;
            }
        }
        else if (playerLeaveArea == null)
        {
            Debug.Log("because of this!");
            currState = enemyState.enemyIdle;
        }
    }

    //function to draw on screen gizmos to better visualize the enemy state areas
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, leavePlayerRange);
    }
}
