using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    Animator animator;
    BoxCollider boxCollider;
    void Start()
    {
        boxCollider = weapon.GetComponentInChildren<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            normalizedTime %= 1;
            if (normalizedTime < 0.50f && normalizedTime > 0.25f && !animator.IsInTransition(0))
            {
                boxCollider.enabled = true;
            }
            else
            {
                boxCollider.enabled = false;
            }

        }
    }
}
