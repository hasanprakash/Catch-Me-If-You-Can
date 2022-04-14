using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Aim : MonoBehaviour
{
    [SerializeField] Transform mainPlayer;
    [SerializeField] float rotationSpeed;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 to = mainPlayer.position;
        to.y = 0f;
        Quaternion yAngle = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, yAngle, rotationSpeed * Time.deltaTime);
    }
}
