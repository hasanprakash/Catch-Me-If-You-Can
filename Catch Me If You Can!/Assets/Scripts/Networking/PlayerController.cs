using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Cinemachine;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

    private NetworkVariable<float> networkMovement = new NetworkVariable<float>();

    private NetworkVariable<bool> networkJump = new NetworkVariable<bool>();

    private NetworkVariable<bool> networkAttack = new NetworkVariable<bool>();

    private NetworkVariable<float> networkJumpSpeed = new NetworkVariable<float>();

    [SerializeField] float forwardSpeed = 30f;
    [SerializeField] float backwardSpeed = 10f;
    [SerializeField] float turnSpeed = 10f;
    Animator animator;
    NetworkObject networkPlayer;
    Rigidbody rb;
    NetworkRigidbody networkrb;
    Vector3 newPosition = Vector3.zero;
    Quaternion newRotation = Quaternion.identity;
    Jump jump;

    float newMovement = 0f;
    bool newJump = false;
    bool newAttack = false;

    Vector3 oldPosition = Vector3.zero;
    Quaternion oldRotation = Quaternion.identity;
    float oldMovement = 0f;
    bool oldJump = false;

    float movementValue = 0f;
    float movementChangeSpeed = 50f;

    // Cinemachine Camera
    public CinemachineFreeLook freeLookCamera;
    Camera mainCamera;
    float forward;

    // Jump Variables
    bool isGrounded = true;
    bool isFalling = false;


    private void Start()
    {
        mainCamera = Camera.main;
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        animator = GetComponent<Animator>();
        jump = GetComponent<Jump>();
        networkPlayer = GetComponent<NetworkObject>();
        rb = networkPlayer.GetComponent<Rigidbody>();
        networkrb = GetComponent<NetworkRigidbody>();
        if(IsServer)
            networkJumpSpeed.Value = 8f;
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            freeLookCamera.Priority = 20;
        }
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }
        ClientMoveAndRotate();
        ClientVisuals();
        //Debug.Log(networkPosition.Value + " " + networkRotation.Value);
    }

    void ClientInput()
    {
        forward = Input.GetAxis("Vertical");
        newMovement = forward;
        newJump = Input.GetKeyDown(KeyCode.Space);
        newAttack = Input.GetKey(KeyCode.X);

        if (forward > 0)
            newPosition = newPosition + (transform.forward * forward * forwardSpeed * Time.deltaTime);
        else
            newPosition = newPosition + (transform.forward * forward * backwardSpeed * Time.deltaTime);

        float yAngle = mainCamera.transform.rotation.eulerAngles.y;
        newRotation = Quaternion.Slerp(newRotation, Quaternion.Euler(0, yAngle, 0), turnSpeed * Time.deltaTime);

        if (newPosition != oldPosition || newRotation != oldRotation)
        {
            UpdateClientPositionAndRotationServerRpc(newPosition, newRotation);
            oldPosition = newPosition;
            oldRotation = newRotation;
        }
        UpdateClientMovementServerRpc(newMovement);
        if (oldMovement != newMovement)
        {
            oldMovement = newMovement;
            //UpdateClientMovementServerRpc(newMovement);
        }
        if (oldJump != newJump)
        {
            UpdateClientJumpServerRpc(newJump);
            Debug.Log("Updated Jump");
            oldJump = newJump;
        }
        UpdateClientAttackServerRpc(newAttack);
    }
    
    void ClientMoveAndRotate()
    {
        //transform.position = networkPosition.Value;
        transform.rotation = networkRotation.Value;
    }

    void ClientVisuals()
    {
        //Debug.Log(networkMovement.Value + " " + newMovement);
        movementValue = networkMovement.Value;

        if (movementValue < 0)
            animator.SetFloat("inputX", -1f);
        else if (movementValue > 0)
            animator.SetFloat("inputX", 1f);
        else if (movementValue == 0)
            animator.SetFloat("inputX", 0f);

        if (animator.GetBool("isAttacking") != networkAttack.Value)
            animator.SetBool("isAttacking", networkAttack.Value);

        Debug.Log(networkJump.Value + " " + isGrounded);
        if(networkJump.Value && isGrounded)
        {
            Debug.Log("Jump from network");
            animator.SetBool("isJumping", true);
            isGrounded = false;
            rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
        }
        if(IsClient && IsOwner && jump.CheckIsGrounded() && !isGrounded)
        {
            newJump = false;
            UpdateClientJumpServerRpc(false);
            isGrounded = true;
        }
    }

    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 position, Quaternion rotation)
    {
        //networkPosition.Value = position;
        networkRotation.Value = rotation;
    }

    [ServerRpc]
    public void UpdateClientJumpServerRpc(bool jump)
    {
        networkJump.Value = jump;
    }

    [ServerRpc]
    public void UpdateClientMovementServerRpc(float movement)
    {
        networkMovement.Value = movement;
    }

    [ServerRpc]
    public void UpdateClientAttackServerRpc(bool attack)
    {
        networkAttack.Value = attack;
    }








    private void FixedUpdate()
    {
        if(jump.CheckIsGrounded())
        {
            //isGrounded = true;
            if (isFalling)
            {
                animator.SetBool("isJumping", false);
            }
            animator.SetBool("isFalling", false);
            isFalling = false;
        }
        else
        {
            //isGrounded = false;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", true);
            isFalling = true;
        }
    }
}
