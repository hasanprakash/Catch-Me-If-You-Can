using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class Net_PlayerJump : NetworkBehaviour
{
    [SerializeField] Text horizontalSpeedText;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float maxJumpHorizontalSpeed = 150f;
    private NetworkVariable<bool> myNetworkJump = new NetworkVariable<bool>();
    private NetworkVariable<bool> isNetworkJumpLocked = new NetworkVariable<bool>();
    NetworkObject networkObject;
    Animator animator;
    Rigidbody rb;
    GroundStatus groundStatus;
    Net_PlayerMovement myPlayerMovement;

    bool IsJumping = false;
    bool IsFalling = false;
    bool IsGrounded = true;
    bool oldJumpingValue = false;
    bool isJumpLocked = true;

    float jumpHorizontalSpeed = 150f;
    float maxJumpVelocity = 5f;

    private void Start()
    {
        groundStatus = GetComponent<GroundStatus>();
        myPlayerMovement = GetComponent<Net_PlayerMovement>();
        IsGrounded = groundStatus.IsGrounded();
        animator = GetComponent<Animator>();
        networkObject = GetComponent<NetworkObject>();
        rb = networkObject.GetComponent<Rigidbody>();
        UpdatePlayerJumpLockedServerRpc(true);
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            TakeNetworkInput();
        }
        ReflectNetworkData();
    }

    void TakeNetworkInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) && animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            IsJumping = true;
            isJumpLocked = false;
            UpdatePlayerJumpLockedServerRpc(false);
        }
        if (oldJumpingValue != IsJumping)
        {
            UpdatePlayerJumpServerRpc(IsJumping);
            oldJumpingValue = IsJumping;
        }
    }

    void ReflectNetworkData()
    {
        if (IsGrounded && !isNetworkJumpLocked.Value && myNetworkJump.Value && IsServer)
        {
            animator.SetBool("isJumping", myNetworkJump.Value);
            isJumpLocked = true;
            isNetworkJumpLocked.Value = true;
            Vector3 direction = Vector3.up * 8f;
            rb.AddForce(direction, ForceMode.Impulse);
        }
        if (IsServer && myNetworkJump.Value)
        {
            Vector3 movingDirection = transform.forward * jumpHorizontalSpeed;
            movingDirection.y = 0f;
            rb.AddForce(movingDirection * Time.deltaTime, ForceMode.Impulse);
        }
        if((IsServer && !myNetworkJump.Value) || (IsClient && !IsJumping))
        {
            CalculateJumpHorizontalSpeed();
        }
    }

    void CalculateJumpHorizontalSpeed()
    {
        jumpHorizontalSpeed = (myPlayerMovement.playerVelocity * maxJumpHorizontalSpeed) / maxJumpVelocity;
        horizontalSpeedText.text = "Horizontal Speed = " + Math.Round(jumpHorizontalSpeed);
    }


    private void FixedUpdate()
    {
        IsGrounded = groundStatus.IsGrounded();
        if (IsGrounded)
        {
            if (IsFalling)
            {
                IsJumping = false;
                animator.SetBool("isJumping", IsJumping);
            }
            IsFalling = false;
            animator.SetBool("isFalling", IsFalling);
        }
        else
        {
            IsJumping = true;
            IsFalling = true;
            animator.SetBool("isFalling", IsFalling);
        }
    }

    [ServerRpc]
    void UpdatePlayerJumpServerRpc(bool IsJumping)
    {
        myNetworkJump.Value = IsJumping;
    }

    [ServerRpc]
    public void UpdatePlayerJumpLockedServerRpc(bool jumpLocked)
    {
        isNetworkJumpLocked.Value = jumpLocked;
    }
}
