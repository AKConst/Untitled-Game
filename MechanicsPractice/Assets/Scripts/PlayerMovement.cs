using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float movSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;

    private Vector2 mousePos;

    [Header("Dash Settings")]
    private Vector2 dashDir;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private bool isDashing = false;
    private bool canDash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing) return;

        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            rb.linearVelocity = new Vector2(0, 0);
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDir = (mousePos - (Vector2)transform.position).normalized;
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = new Vector2(speedX, speedY);
    }

    private IEnumerator Dash()
    {
        Debug.Log("Start Dash!");
        isDashing = true;
        canDash = false;

        rb.linearVelocity = dashDir * dashDistance;
        yield return new WaitForSeconds(dashDuration);

        Debug.Log("End Dash!");
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        Debug.Log("Dash Cooldown Expired!");
    }
    
}
