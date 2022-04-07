using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    float move = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("Vertical");
        if(move != 0)
        {
            animator.SetFloat("inputX", move);
        }
        else
        {
            animator.SetFloat("inputX", move);
        }
    }
}
