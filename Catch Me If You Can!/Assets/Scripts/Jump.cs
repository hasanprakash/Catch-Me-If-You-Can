using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class Jump : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    //[SerializeField] float distanceToGround = 0.1f;
    [SerializeField] float maxVelocity = 5f;
    [SerializeField] Text groundStatusText;
    [SerializeField] Text velocityText;
    [SerializeField] Text horizontalSpeedText;
    [SerializeField] float jumpSpeed = 50f;
    [SerializeField] float maxJumpHorizontalSpeed = 150f;
    [SerializeField] GameObject twoBoneIK;
    [Range(0, 1)]  public float batWeight = 0.5f;
    TwoBoneIKConstraint ikconstraint;
    float jumpHorizontalSpeed = 150f;
    Animator animator;
    Rigidbody rb;
    float ikconstraintSpeed = 1.5f;
    public double velocityOfPlayer = 5f;
    bool isGrounded = true;
    bool isJumping = false;
    bool isFalling = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ikconstraint = twoBoneIK.GetComponent<TwoBoneIKConstraint>();
    }

    void Update()
    {
        ikconstraint.weight = batWeight;
        displayPlayerInfo();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //rb.velocity = direction;
            animator.SetBool("isJumping", true);
            isJumping = true;
            Vector3 direction = Vector3.up * jumpSpeed;
            isGrounded = false;
            rb.AddForce(direction, ForceMode.Impulse);
        }

        if(!CheckIsGrounded())
        {
            //batWeight = Mathf.Clamp(batWeight - ikconstraintSpeed * Time.deltaTime, 0f, 1f);
            Vector3 movingDirection = transform.forward * jumpHorizontalSpeed;
            movingDirection.y = 0f;
            rb.AddForce(movingDirection * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            //batWeight = Mathf.Clamp(batWeight + ikconstraintSpeed * Time.deltaTime, 0f, batWeight);
            calculatePlayerVelocity();
        }
    }

    void FixedUpdate()
    {
        if (CheckIsGrounded())
        {
            groundStatusText.text = "You are Grounded";
            isGrounded = true;
            if(isFalling)
            {
                animator.SetBool("isJumping", false);
                isJumping = false;
            }
            animator.SetBool("isFalling", false);
            isFalling = false;
        }
        else
        {
            groundStatusText.text = "In Air";
            isGrounded = false;
            animator.SetBool("isJumping", true);
            isJumping = true;
            animator.SetBool("isFalling", true);
            isFalling = true;
        }
    }

    public bool CheckIsGrounded()
    {
        // with Capsule
        Vector3 startingPosition = groundCheck.position;
        Vector3 endingPosition = startingPosition - new Vector3(0, 0.1f, 0);
        return Physics.CheckCapsule(startingPosition, endingPosition, 0.1f, ground);

        // with Sphere
        //return Physics.CheckSphere(groundCheck.position, distanceToGround, ground);
    }

    void calculatePlayerVelocity()
    {
        Vector3 playerVelocity = rb.velocity;
        playerVelocity.y = 0f;
        float playerMagnitude = playerVelocity.magnitude;
        jumpHorizontalSpeed = (playerMagnitude * maxJumpHorizontalSpeed) / maxVelocity;
    }

    void displayPlayerInfo()
    {
        Vector3 playerVelocity = rb.velocity;
        playerVelocity.y = 0f;
        float playerMagnitude = playerVelocity.magnitude;
        velocityText.text = "Velocity = " + Math.Round(playerMagnitude, 1) + " u/s";
        velocityOfPlayer = Math.Round(playerMagnitude, 1);
        horizontalSpeedText.text = "Horizontal Speed = " + Math.Round(jumpHorizontalSpeed);
    }
}
