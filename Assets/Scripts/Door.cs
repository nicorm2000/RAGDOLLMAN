using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int requiredSwitchesToOpen = 1;

    [SerializeField] Animator animator;

    public bool isDoorOpen = false;

    private List<PressureSwitch> currentSwitchesOpen = new();

    public void AddPressureSwitch(PressureSwitch currentSwitch)
    {
        if (!currentSwitchesOpen.Contains(currentSwitch))
        {
            currentSwitchesOpen.Add(currentSwitch);
        }

        TryOpen();
    }

    public void RemovePressureSwitch(PressureSwitch currentSwitch)
    {
        if (currentSwitchesOpen.Contains(currentSwitch))
        {
            currentSwitchesOpen.Remove(currentSwitch);
        }

        TryOpen();
    }

    private void TryOpen()
    {
        if (currentSwitchesOpen.Count == requiredSwitchesToOpen)
        {
            OpenDoor();
        }
        else if (currentSwitchesOpen.Count < requiredSwitchesToOpen)
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        if (!isDoorOpen)
        {
            animator.SetBool("Up", true);
        }

        isDoorOpen = true;
    }

    private void CloseDoor()
    {
        if (isDoorOpen)
        {
            animator.SetBool("Up", false);
        }

        isDoorOpen = false;
    }
}
