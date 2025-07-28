using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyGeneric : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private float timeToAttack;
    [SerializeField] private float atkHitRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform attackPos;
    [SerializeField] private GameObject attackSprite;

    private bool canAttack = true;
    private bool isAttacking = false;

    private Collider2D playerLeaveArea;
    private Collider2D playerDetectArea;
    private Collider2D playerAttackArea;

    [Header("Detection Settings")]
    [SerializeField] private float DetectRange;
    [SerializeField] private float LeavePlayerRange;
    [SerializeField] private float AttackRange;

    [SerializeField] private MonoBehaviour[] chaseScripts;
    private MonoBehaviour atkAlignScript; 

    private enum enemyState { enemyChase, enemyIdle, enemyAttack };
    private enemyState currState;

    void Start()
    {
        atkAlignScript = this.GetComponentInChildren<FacePlayer>();
        foreach (MonoBehaviour item in chaseScripts)
        {
            item.enabled = false;
        }
    }

    void Update()
    {
        updateState();

        switch (currState)
        {
            case enemyState.enemyChase:
                foreach (MonoBehaviour item in chaseScripts)
                {
                    item.enabled = true;
                }
                break;
            case enemyState.enemyAttack:
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
                foreach (MonoBehaviour item in chaseScripts)
                {
                    item.enabled = false;
                }
                break;
        }
    }

    private void updateState()
    {
        playerLeaveArea = Physics2D.OverlapCircle(transform.position, LeavePlayerRange, whatIsPlayer);
        playerDetectArea = Physics2D.OverlapCircle(transform.position, DetectRange, whatIsPlayer);
        playerAttackArea = Physics2D.OverlapCircle(transform.position, AttackRange, whatIsPlayer);
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
        canAttack = false;
        isAttacking = true;

        atkAlignScript.enabled = false;

        GameObject eAtkIndicatorInstance = Instantiate(attackIndicator, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        eAtkIndicatorInstance.transform.parent = transform;
        yield return new WaitForSeconds(timeToAttack);
        Destroy(eAtkIndicatorInstance);

        GameObject eAtkHitAreaInstance = Instantiate(attackSprite, attackPos.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(eAtkHitAreaInstance);

        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos.position, atkHitRange, whatIsPlayer);
        if (hitPlayer != null)
        {
            Destroy(hitPlayer.gameObject);
            this.enabled = false;
            GameManagerScript.instance.ChangeLevel(4);
        }

        canAttack = true;
        isAttacking = false;

        atkAlignScript.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LeavePlayerRange);
    }
}
