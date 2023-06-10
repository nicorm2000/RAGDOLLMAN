using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int requiredSwitchesToOpen = 1;

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
            Debug.Log("Open");
        }

        isDoorOpen = true;
    }

    private void CloseDoor()
    {
        if (!isDoorOpen)
        {
            Debug.Log("Close");
        }

        isDoorOpen = false;
    }
}
