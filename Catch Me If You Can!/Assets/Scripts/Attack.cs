using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject hero;
    BatWeight batWeightObject;
    GroundStatus groundStatus;
    Animation anime;
    Animator animator;
    bool isGrounded = true;
    bool isLegacyAttacking = false;
    bool isHumanoidAttacking = false;
    void Start()
    {
        anime = GetComponent<Animation>();
        batWeightObject = hero.GetComponent<BatWeight>();
        groundStatus = hero.GetComponent<GroundStatus>();
        animator = hero.GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = groundStatus.IsGrounded();
        isHumanoidAttacking = animator.GetBool("isAttacking");
        isLegacyAttacking = anime.isPlaying;
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("isAttacking", false);
        }*/
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anime.Play("Aim");
        }*/
        //else if(Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    anime.CrossFade("SpecialAttack");
        //}

        if (isHumanoidAttacking && isGrounded)
        {
            ChangeBatWeight(0f, 1f, -1, 5f);
        }
        else if (isLegacyAttacking && isGrounded)
        {
            ChangeBatWeight(0f, 1f, 1, 5f);
        }
        else if (isGrounded)
        {
            if (batWeightObject.batWeight > 0.5f)
            {
                ChangeBatWeight(0.5f, 1f, -1, 2f);
            }
            else
            {
                ChangeBatWeight(0f, 0.5f, 1, 2f);
            }
        }
        else if(isGrounded)
        {
            ChangeBatWeight(0.5f, 1f, -1, 2f);
        }
        else
        {
            ChangeBatWeight(0f, 1f, -1, 1.5f);
        }
    }


    void ChangeBatWeight(float minValue, float maxValue, int status, float speed)
    {
        batWeightObject.batWeight = Mathf.Clamp(batWeightObject.batWeight + speed * status * Time.deltaTime, minValue, maxValue);
    } 
}
