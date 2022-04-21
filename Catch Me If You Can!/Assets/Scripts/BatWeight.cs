using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BatWeight : MonoBehaviour
{
    [SerializeField] GameObject twoBoneIK;
    TwoBoneIKConstraint ikconstraint;
    [Range(0, 1)] public float batWeight = 0.5f;

    private void Start()
    {
        ikconstraint = twoBoneIK.GetComponent<TwoBoneIKConstraint>();
    }

    private void Update()
    {
        ikconstraint.weight = batWeight;
    }
}
