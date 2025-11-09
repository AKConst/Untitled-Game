using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Vector3 mousePos; //variable for our mouse position
    [SerializeField] private Transform attackPos; //reference to the position of the place our attack will spawn at
    [SerializeField] private LayerMask whatIsEnemy; //layer mask to know what is enemy for detection
    [SerializeField] private float attackRange; //the range of our attack
    [SerializeField] private GameObject attackIndicator; //reference to the attack indicator
    [SerializeField] public bool isAttacking = false;
    [SerializeField] private GameObject AttackCircle;
    private Animator animator; //animator is needed to do attack animation  

    private void Start()
    {
        animator = GetComponentInParent<Animator>();        
    }
    void Update()
    {
        //calculate the value of the mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //set the appropriate rotation to where the player is looking (towards the mouse, from the player)
        Vector3 rotation = mousePos - transform.position;
        float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, roZ);

        //check if the player pressed mouse1
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //if so, we instantiate the attack indicator sprite, we then start a coroutine to delete it after
            //an appropriate amount of time
            //GameObject indicatorInstance = Instantiate(attackIndicator, attackPos.position, Quaternion.identity);
            //StartCoroutine(deleteIndicator(indicatorInstance));
            isAttacking = true;
            //we check if any enemies fell within our attack range
            GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.identity);
            //Collider2D[] enemiesEffected = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
            StartCoroutine(deleteIndicator(Attack));
            animator.SetBool("Attack", true);

        }
        else
        {
            animator.SetBool("Attack", false);
        }
            isAttacking = false;
        
    }

    //drawing gizmos to better visualize the attack ranges etc.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator deleteIndicator(GameObject ind)
    {
        //wait for 0.5 seconds before deleting the attack sprite
        yield return new WaitForSeconds(0.5f);
        Destroy(ind);
    }

}
