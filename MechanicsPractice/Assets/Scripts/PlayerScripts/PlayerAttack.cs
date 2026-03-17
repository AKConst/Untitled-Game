using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform slashParent;
    private Vector3 mousePos; //variable for our mouse position
    [SerializeField] private Transform attackPos; //reference to the position of the place our attack will spawn at
    [SerializeField] private LayerMask whatIsEnemy; //layer mask to know what is enemy for detection
    [SerializeField] private float attackRange; //the range of our attack
    [SerializeField] private GameObject attackIndicator; //reference to the attack indicator
    [SerializeField] public bool isAttacking = false;
    [SerializeField] private GameObject AttackCircle;
    [SerializeField] private GameObject FinalSlash;
    [SerializeField] private float setCooldown = 1;
    [SerializeField] private float setPurpleCooldown = 5;
    private bool isPurpleAttack = false;
    private float cooldown = 0;
    private float purpleCooldown = 0;
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
        Vector2 rotation = mousePos - transform.position;
        float roZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, roZ);

        if (!isAttacking)
        {
            //check if the player pressed on both mouse buttons
            //if (Input.GetMouseButton(0) && Input.GetMouseButton(1))

           
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //initating the attack by setting the proper animator peramaters, set stringval to 0 so it does firstAttack
                animator.SetFloat("MouseX", rotation.x);
                animator.SetFloat("MouseY", rotation.y);
                animator.SetBool("Attack", true);
                animator.SetInteger("StringVal", 0);



                //if so, we instantiate the attack indicator sprite, we then start a coroutine to delete it after
                //an appropriate amount of time
                isAttacking = true;

                GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                Attack.GetComponent<Animator>().SetBool("RAttack", true);
                StartCoroutine(deleteIndicator(Attack));


            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //initating the attack by setting the proper animator peramaters, increasing the PSC and starting our timer.
                animator.SetFloat("MouseX", rotation.x);
                animator.SetFloat("MouseY", rotation.y);
                animator.SetBool("Attack", true);
                animator.SetInteger("StringVal", 1);

                isAttacking = true;


                GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                Attack.GetComponent<Animator>().SetBool("BAttack", true);
                Attack.GetComponent<SpriteRenderer>().flipX = true;
                StartCoroutine(deleteIndicator(Attack));
            }

             
            cooldown = setCooldown; 
        }
        else
        {
            animator.SetBool("Attack", false);
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
            {
                isAttacking = false;
            }
        }

        if (!isPurpleAttack)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //initating the attack by setting the proper animator peramaters, set stringval to 2 so it does finalAttack
                animator.SetFloat("MouseX", rotation.x);
                animator.SetFloat("MouseY", rotation.y);
                animator.SetBool("Purple_Attack", true);
                animator.SetInteger("StringVal", 2);



                //if so, we instantiate the attack indicator sprite, we then start a coroutine to delete it after
                //an appropriate amount of time
                isPurpleAttack = true;


                GameObject FinalAttack = Instantiate(FinalSlash, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                // running a function for the final attack to follow along with the player
                FinalAttack.GetComponent<FinalSlash>();
                StartCoroutine(deleteIndicator(FinalAttack));
            }
            
            purpleCooldown = setPurpleCooldown;
        }
        else
        {
            animator.SetBool("Purple_Attack", false);
            purpleCooldown -= Time.deltaTime;
            if (purpleCooldown < 0)
            {
                isPurpleAttack = false;
            }
        }

    }

    private IEnumerator deleteIndicator(GameObject ind)
    {
        //wait for 0.5 seconds before deleting the attack sprite
        yield return new WaitForSeconds(0.5f); //0.5
        Destroy(ind);
    }

    /*
    public void SetPSC(int val)
    {
        this.PSC = val;
    }
    public int GetPSC()
    {
        return this.PSC;
    }
    */

    //function to dash on the last attack of the combo. Called from the player movement script
    
}
