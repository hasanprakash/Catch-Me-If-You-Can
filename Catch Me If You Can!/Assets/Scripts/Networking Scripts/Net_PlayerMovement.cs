using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class Net_PlayerMovement : NetworkBehaviour
{
    [SerializeField] Text velocityText;
    private NetworkVariable<float> networkMovement = new NetworkVariable<float>();
    Animator animator;
    NetworkObject networkObject;
    Rigidbody rb;
    [HideInInspector]
    public float playerVelocity = 5f;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        networkObject = GetComponent<NetworkObject>();
        rb = networkObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        CalculateAndDisplayVelocity();
        if (IsClient && IsOwner)
        {
            TakeNetworkInput();
        }
        ReflectNetworkData();
    }

    void TakeNetworkInput()
    {
        float direction = Input.GetAxis("Vertical");
        UpdatePlayerMovementServerRpc(direction);
    }
    
    void ReflectNetworkData()
    {
        animator.SetFloat("inputX", networkMovement.Value);
    }

    void CalculateAndDisplayVelocity()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        playerVelocity = (float) Math.Round(velocity.magnitude, 1);
        velocityText.text = "Velocity = " + playerVelocity + " u/s";
    }

    [ServerRpc]
    void UpdatePlayerMovementServerRpc(float direction)
    {
        networkMovement.Value = direction;
    }
}
