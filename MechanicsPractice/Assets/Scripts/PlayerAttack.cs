using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Vector3 mousePos;
    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float attackRange;

    [SerializeField] private GameObject attackIndicator;

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, roZ);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject indicatorInstance = Instantiate(attackIndicator, attackPos.position, Quaternion.identity);
            StartCoroutine(deleteIndicator(indicatorInstance));

            Collider2D[] enemiesEffected = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
            foreach(Collider2D enemy in enemiesEffected)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator deleteIndicator(GameObject ind)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(ind);
    }
}
