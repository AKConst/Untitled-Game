using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class MantisSlash : EnemyAttackSO
{
    public override IEnumerator Attack(GameObject parent)
    {
        //setting our booleans for the attacking status
        parent.GetComponent<EnemyGeneric>().canAttack = false;
        parent.GetComponent<EnemyGeneric>().isAttacking = true;

        //we instantiate the enemy attack indicator above the enemy and parent it to the enemys transform
        int indicator = Random.Range(0, 2);
        switch (indicator) {
            case 0:
                attackObjects[0].GetComponent<EnemyAttack>().attackColor = "red";
                break;
            case 1:
                attackObjects[0].GetComponent<EnemyAttack>().attackColor = "blue";
                break;
            case 2:
                attackObjects[0].GetComponent<EnemyAttack>().attackColor = "purple";
                break;
        }

        GameObject eAtkIndicatorInstance = Instantiate(indicators[indicator], 
            new Vector3(parent.transform.position.x, parent.transform.position.y + 1, parent.transform.position.z), Quaternion.identity);
        eAtkIndicatorInstance.transform.parent = parent.transform;
        //we wait for the attack chargeup time to finish before we delete the indicator and proceed with the attack
        yield return new WaitForSeconds(chargeUpTime);

        Destroy(eAtkIndicatorInstance);

        Transform attackSpawnPos = parent.transform.GetChild(0).GetChild(0);

        GameObject Attack = Instantiate(attackObjects[0], attackSpawnPos.position, Quaternion.identity);
        Attack.GetComponent<EnemyAttack>().enemyInstance = parent;
        yield return new WaitForSeconds(0.35f); //how long the attack itself lasts in miliseconds

        Destroy(Attack);

        parent.GetComponent<EnemyGeneric>().isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);

        //setting our attack status booleans
        parent.GetComponent<EnemyGeneric>().canAttack = true;
    }
}
