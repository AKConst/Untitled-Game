using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class RedBallMelee : EnemyAttackSO
{
    public override IEnumerator Attack(GameObject parent)
    {
        //setting our booleans for the attacking status
        parent.GetComponent<EnemyGeneric>().canAttack = false;
        parent.GetComponent<EnemyGeneric>().isAttacking = true;

        //we instantiate the enemy attack indicator above the enemy and parent it to the enemys transform
        GameObject eAtkIndicatorInstance = Instantiate(indicators[0], 
            new Vector3(parent.transform.position.x, parent.transform.position.y + 1, parent.transform.position.z), Quaternion.identity);
        eAtkIndicatorInstance.transform.parent = parent.transform;
        //we wait for the attack chargeup time to finish before we delete the indicator and proceed with the attack
        yield return new WaitForSeconds(chargeUpTime);

        Destroy(eAtkIndicatorInstance);

        Transform attackSpawnPos = parent.transform.GetChild(0).GetChild(0);

        GameObject Attack = Instantiate(attackObjects[0], attackSpawnPos.position, Quaternion.identity);
        Attack.GetComponent<EnemyAttack>().enemyInstance = parent;
        yield return new WaitForSeconds(0.15f);

        Destroy(Attack);

        parent.GetComponent<EnemyGeneric>().isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);

        //setting our attack status boolean
        parent.GetComponent<EnemyGeneric>().canAttack = true;
    }
}
