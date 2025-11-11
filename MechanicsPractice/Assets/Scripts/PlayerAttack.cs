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
    [SerializeField] private GameObject FinalSlash;
    private Animator animator; //animator is needed to do attack animation  
    private int PSC = 0; //player string counter
    [SerializeField] private MonoBehaviour stringTimer; //reference for our timer

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

        //check if the player pressed mouse1
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //initating the attack by setting the proper animator peramaters, increasing the PSC and starting our timer.
            animator.SetBool("Attack", true);
            animator.SetInteger("StringVal", PSC);
            PSC++;
            stringTimer.enabled = true;
            stringTimer.GetComponent<StringTimer>().resetTimer();

            //if so, we instantiate the attack indicator sprite, we then start a coroutine to delete it after
            //an appropriate amount of time
            isAttacking = true;
            if (PSC < 3)
            {
                GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                Attack.GetComponent<Animator>().SetBool("RAttack", true);
                StartCoroutine(deleteIndicator(Attack));
            }
            else
            {
                AttackDash(rotation.normalized, 30);
                GameObject FinalAttack = Instantiate(FinalSlash, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                StartCoroutine(deleteIndicator(FinalAttack));
                PSC = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //initating the attack by setting the proper animator peramaters, increasing the PSC and starting our timer.
            animator.SetBool("Attack", true);
            animator.SetInteger("StringVal", PSC);
            PSC++;
            stringTimer.enabled = true;
            stringTimer.GetComponent<StringTimer>().resetTimer();

            isAttacking = true;
            if (PSC < 3)
            {
                GameObject Attack = Instantiate(AttackCircle, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                Attack.GetComponent<Animator>().SetBool("BAttack", true);
                Attack.GetComponent<SpriteRenderer>().flipX = true;
                StartCoroutine(deleteIndicator(Attack));
            }
            else
            {
                AttackDash(rotation.normalized, 30);
                GameObject FinalAttack = Instantiate(FinalSlash, attackPos.position, Quaternion.Euler(new Vector3(0, 0, roZ - 90)));
                StartCoroutine(deleteIndicator(FinalAttack));
                PSC = 0;
            }
        }
        else
        {
            animator.SetBool("Attack", false);
        }
        isAttacking = false;
    }

    private IEnumerator deleteIndicator(GameObject ind)
    {
        //wait for 0.5 seconds before deleting the attack sprite
        yield return new WaitForSeconds(0.5f);
        Destroy(ind);
    }

    //getters and setters for the PSC
    public void SetPSC(int val)
    {
        this.PSC = val;
    }
    public int GetPSC()
    {
        return this.PSC;
    }

    //function to dash on the last attack of the combo. Called from the player movement script
    private void AttackDash(Vector2 dir, int dist)
    {
        StartCoroutine(GetComponentInParent<PlayerMovement>().Dash(dir, dist));
    }
}
