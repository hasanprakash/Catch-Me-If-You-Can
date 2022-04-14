using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    Rigidbody[] rb;
    Collider[] colliders;
    Rigidbody mainRB;
    Collider mainCollider;
    Animator animator;
    Transform weapon;
    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        mainRB = GetComponent<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        weapon = transform.GetChild(11);

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody r in rb)
        {
            r.isKinematic = true;
        }
        mainCollider.enabled = true;
        mainRB.isKinematic = false;
        animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Animation to Ragdoll
        if(Input.GetKeyDown(KeyCode.T))
        {
            EnableRagDoll();
        }
        // Ragdoll to Animation
        if(Input.GetKeyDown(KeyCode.Y))
        {
            DisableRagDoll();
        }

    }

    public void EnableRagDoll()
    {
        foreach (Rigidbody r in rb)
        {
            r.isKinematic = false;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        mainRB.isKinematic = true;
        mainCollider.enabled = false;
        animator.enabled = false;

        weapon.GetComponentInChildren<BoxCollider>().enabled = false;
    }
    public void DisableRagDoll()
    {
        foreach (Rigidbody r in rb)
        {
            r.isKinematic = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        mainRB.isKinematic = false;
        mainCollider.enabled = true;
        animator.enabled = true;
    } 
}
