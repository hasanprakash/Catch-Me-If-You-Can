using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Net_PlayerAttack : NetworkBehaviour
{
    private NetworkVariable<bool> mynetworkAttack = new NetworkVariable<bool>();
    Animator animator;
    bool newAttack = false;
    bool oldAttack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        newAttack = Input.GetKey(KeyCode.X);
        if (oldAttack != newAttack)
        {
            UpdateMyClientAttackServerRpc(newAttack);
            oldAttack = newAttack;
        }
    }

    void ReflectNetworkData()
    {
        if (animator.GetBool("isAttacking") != mynetworkAttack.Value)
            animator.SetBool("isAttacking", mynetworkAttack.Value);
    }

    [ServerRpc]
    public void UpdateMyClientAttackServerRpc(bool attack)
    {
        mynetworkAttack.Value = attack;
    }
}
