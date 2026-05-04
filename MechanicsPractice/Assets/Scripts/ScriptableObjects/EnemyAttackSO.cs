using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "Scriptable Objects/EnemyAttackSO")]
public class EnemyAttackSO : ScriptableObject
{
    [Header("Attack Information:")]
    public List<GameObject> attackObjects;
    public int priorityValue;
    public List<GameObject> indicators;
    public float chargeUpTime;
    public float attackCooldown;

    public virtual IEnumerator Attack(GameObject parent)
    {
        yield return new WaitForSeconds(10);
    }
}
