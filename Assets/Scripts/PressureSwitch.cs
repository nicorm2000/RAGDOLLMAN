using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] Door currentDoor;

    [SerializeField] Animator animator;

    private void OnTriggerStay(Collider other)
    {
        currentDoor.AddPressureSwitch(this);
        animator.SetBool("Down", true);
    }

    private void OnTriggerExit(Collider other)
    {
        currentDoor.RemovePressureSwitch(this);
        animator.SetBool("Down", false);
    }
}
