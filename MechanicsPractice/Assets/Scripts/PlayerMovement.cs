using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float movSpeed; //where we can assign value to the players movement speed
    private float speedX, speedY; //values to store the current X, Y speeds
    private Rigidbody2D rb; //the player rigidbody reference to use to assign force to
    private Animator animator;//player animator for animations
    private Vector2 dir;

    //various settings regarding the dash such as the distance, duration and cooldown, as well as the direction
    //which gets calculated when the dash is started
    [Header("Dash Settings")]
    private Vector2 dashDir;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    //booleans to keep track of dashing status
    private bool isDashing = false;
    private bool canDash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //assigning value to our rigidbody variable
        animator = GetComponent<Animator>(); //assigining value to our animator variable
        animator.SetFloat("LastH", -1);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing) return; //if the player is dashing we dont calculate forces for regular movement
        //set last movement values
        if (dir.x != 0 || dir.y != 0)
        {
            animator.SetFloat("LastH", dir.x);
            animator.SetFloat("LastV", dir.y);
        }

        //we calculate the speed on the X and Y axis based on player input, multiplied by the speed.
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        speedX = dir.x * movSpeed;
        speedY = dir.y * movSpeed;
       
        //set new values for movement
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", dir.sqrMagnitude);
        

        //begin the dash on proper input as well as valid status
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            //resetting velocity before performing the dash in order for already existing movement force to not be
            //added onto the dash force
            rb.linearVelocity = new Vector2(0, 0);
            dashDir = new Vector2(speedX, speedY).normalized; //calculating the dash direction based on the axis inputs
            StartCoroutine(Dash(dashDir, dashDistance));//starting our dash in a coroutine
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }
        else
        {
            rb.linearVelocity = new Vector2(speedX, speedY); //applying the force for our regular movement.
        }
    }

    public IEnumerator Dash(Vector2 ddir, float ddist)
    {
        Debug.Log("Start Dash!");
        //setting our dashing status booleans to their appropriate values
        isDashing = true;
        canDash = false;

        // calculating the force that needs to be applied by the direction for the dash ad its distance
        //we then proceed to wait the amount of seconds that the dash should last before proceeding
        rb.linearVelocity = ddir * ddist;
        yield return new WaitForSeconds(dashDuration);

        Debug.Log("End Dash!");
        isDashing = false; //set dashing status to appropriate value
        yield return new WaitForSeconds(dashCooldown); //wait for the dash cooldown to expire
        canDash = true; //set dashing status to appropriate value

        Debug.Log("Dash Cooldown Expired!");
    }
    
}
