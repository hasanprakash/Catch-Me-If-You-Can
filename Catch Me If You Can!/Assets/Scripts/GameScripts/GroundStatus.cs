using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HasanPrakash.Singlestons;
using UnityEngine.UI;

public class GroundStatus : Singleton<GroundStatus>
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] Text groundStatusText;

    public bool IsGrounded()
    {
        Vector3 startingPosition = groundCheck.position;
        Vector3 endingPosition = startingPosition - new Vector3(0, 0.1f, 0);
        return Physics.CheckCapsule(startingPosition, endingPosition, 0.1f, ground);
    }

    private void FixedUpdate()
    {
        if(IsGrounded())
        {
            groundStatusText.text = "You are Grounded";
        }
        else
        {
            groundStatusText.text = "In Air";
        }
    }
}
