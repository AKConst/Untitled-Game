using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ChasePlayerScript : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private float timeToAttack;
    [SerializeField] private float atkHitRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform attackPos;
    [SerializeField] private GameObject attackSprite;

    [Header("LOS Settings")]
    [SerializeField] private float DetectRange;
    [SerializeField] private float AttackRange;

    [SerializeField] private MonoBehaviour[] chaseScripts;

    private enum enemyState {enemyChase, enemyIdle, enemyAttack};
    private enemyState currState;

    private void Start()
    {
        foreach (MonoBehaviour item in chaseScripts)
        {
            item.enabled = false;
        }
    }

    void Update()
    {
        updateState();

        if (currState == enemyState.enemyChase)
        {
            foreach (MonoBehaviour item in chaseScripts)
            {
                if (!item.enabled) item.enabled = true;
            }
            Collider2D playerCol = Physics2D.OverlapCircle((Vector2)transform.position, AttackRange, whatIsPlayer);
            if (playerCol != null)
            {
                currState = enemyState.enemyAttack;
            }
        }
        else if (currState == enemyState.enemyAttack)
        {
            StartCoroutine(EnemyAttack());
            Collider2D playerStillInRange = Physics2D.OverlapCircle((Vector2)transform.position, AttackRange, whatIsPlayer);
            Collider2D playerEscapedFully = Physics2D.OverlapCircle((Vector2)transform.position, DetectRange, whatIsPlayer);
            if (playerStillInRange == null)
            {
                currState = enemyState.enemyChase;
            }
            else if (playerEscapedFully == null)
            {
                currState = enemyState.enemyIdle;
            }
        }
    }

    private IEnumerator EnemyAttack()
    {
        GameObject eAtkIndicatorInstance = Instantiate(attackIndicator, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(timeToAttack);

        GameObject eAtkHitAreaInstance = Instantiate(attackSprite, attackPos.position, Quaternion.identity);
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos.position, atkHitRange, whatIsPlayer);
        if (hitPlayer != null)
        {
            Destroy(hitPlayer.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
        Destroy(eAtkHitAreaInstance);
    }

    private void updateState()
    {
        Collider2D playerCol = Physics2D.OverlapCircle((Vector2)transform.position, DetectRange, whatIsPlayer);
        if (playerCol != null)
        {
            currState = enemyState.enemyChase;
        }
        else
        {
            currState = enemyState.enemyIdle;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);

        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(attackPos.position, AttackRange);
    }
}
