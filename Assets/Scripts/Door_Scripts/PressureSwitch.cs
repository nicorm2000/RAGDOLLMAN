using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] Door currentDoor;

    [SerializeField] Animator animator;

    [SerializeField] LayerMask playerLayer;

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentDoor.AddPressureSwitch(this);
            animator.SetBool("Down", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentDoor.RemovePressureSwitch(this);
            animator.SetBool("Down", false);
        }
    }
}
