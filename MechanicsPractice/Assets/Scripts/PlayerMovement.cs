using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float movSpeed; //where we can assign value to the players movement speed
    private float speedX, speedY; //values to store the current X, Y speeds
    private Rigidbody2D rb; //the player rigidbody reference to use to assign force to

    //private Vector2 mousePos; //now obsolete due to dashing change to being keyboard oriented

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
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing) return; //if the player is dashing we dont calculate forces for regular movement

        //we calculate the speed on the X and Y axis based on player input, multiplied by the speed.
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;

        //begin the dash on proper input as well as valid status
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            //resetting velocity before performing the dash in order for already existing movement force to not be
            //added onto the dash force
            rb.linearVelocity = new Vector2(0, 0);
            dashDir = new Vector2(speedX, speedY).normalized; //calculating the dash direction based on the axis inputs
            StartCoroutine(Dash());//starting our dash in a coroutine
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = new Vector2(speedX, speedY); //applying the force for our regular movement.
    }

    private IEnumerator Dash()
    {
        Debug.Log("Start Dash!");
        //setting our dashing status booleans to their appropriate values
        isDashing = true;
        canDash = false;

        // calculating the force that needs to be applied by the direction for the dash ad its distance
        //we then proceed to wait the amount of seconds that the dash should last before proceeding
        rb.linearVelocity = dashDir * dashDistance;
        yield return new WaitForSeconds(dashDuration);

        Debug.Log("End Dash!");
        isDashing = false; //set dashing status to appropriate value
        yield return new WaitForSeconds(dashCooldown); //wait for the dash cooldown to expire
        canDash = true; //set dashing status to appropriate value

        Debug.Log("Dash Cooldown Expired!");
    }
    
}
