using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyAttackStateGeneric : MonoBehaviour
{
    [Header("Attack Settings:")]
    [SerializeField] private List<EnemyAttackSO> attackList;
    [SerializeField] private bool isHybrid;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        if (!isHybrid)
        {
            //add rng calculations
            int randValue = Random.Range(1, 100);
            int currMax = 0;
            for (int i = 0; i < attackList.Count; i++)
            {
                if (attackList[i].priorityValue >= randValue)
                {
                    currMax = i;
                }
            }
            animator.SetInteger("AttackIndex", currMax);
            StartCoroutine(attackList[currMax].Attack(gameObject));
        }
        else
        {
            //calculations for hybrid enemies
        }
    }
}
