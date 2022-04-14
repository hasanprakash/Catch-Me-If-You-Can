using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject multiAim;
    [SerializeField] GameObject hero;
    Jump jump;
    MultiAimConstraint multiAimConstraint;
    Animation anime;
    Animator animator;
    float bodyRotationSpeed = 1.3f;
    float ikconstraintSpeed = 10f;
    float batWeight;
    bool isGrounded = true;
    bool isLegacyAttacking = false;
    bool isHumanoidAttacking = false;
    void Start()
    {
        multiAimConstraint = multiAim.GetComponent<MultiAimConstraint>();
        anime = GetComponent<Animation>();
        jump = hero.GetComponent<Jump>();
        batWeight = jump.batWeight;
        animator = hero.GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = jump.CheckIsGrounded();
        isHumanoidAttacking = animator.GetBool("isAttacking");
        isLegacyAttacking = anime.isPlaying;
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("isAttacking", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anime.Play("Aim");
        }
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
            if (jump.batWeight > 0.5f)
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


    void ChangeBatWeight(float a, float b, int status, float speed)
    {
        jump.batWeight = Mathf.Clamp(jump.batWeight + speed * status * Time.deltaTime, a, b);
    } 
}
