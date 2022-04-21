using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class Net_PlayerRotation : NetworkBehaviour
{
    private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();
    [SerializeField] float playerRotationSpeed = 20f;
    CinemachineFreeLook freeLookCamera;
    Quaternion newRotation = Quaternion.identity;
    Quaternion oldRotation = Quaternion.identity;
    Animator animator;
    Camera mainCamera;
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            freeLookCamera.Priority = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient && IsOwner)
        {
            TakeNetworkInput();
        }
        ReflectNetworkData();
    }

    void TakeNetworkInput()
    {
        float yAngle = mainCamera.transform.rotation.eulerAngles.y;
        newRotation = Quaternion.Slerp(newRotation, Quaternion.Euler(0, yAngle, 0), playerRotationSpeed * Time.deltaTime);
        UpdatePlayerRotationServerRpc(newRotation);
    }

    void ReflectNetworkData()
    {
        transform.rotation = networkRotation.Value;
    }

    [ServerRpc]
    public void UpdatePlayerRotationServerRpc(Quaternion rotation)
    {
        networkRotation.Value = rotation;
    }
}
