using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of a door object, including its opening and closing based on pressure switches.
/// </summary>
public class Door : MonoBehaviour
{
    [Header("Door Configuration")]
    [SerializeField] private int requiredSwitchesToOpen = 3;
    [SerializeField] private GameObject currentObject;

    private bool isDoorOpen = false;

    // The list of pressure switches currently active on the door
    private List<PressureSwitch> currentSwitchesOpen = new List<PressureSwitch>();

    /// <summary>
    /// Adds the given pressure switch to the list of active switches.
    /// Checks if the door can be opened.
    /// </summary>
    /// <param name="currentSwitch">The pressure switch to add.</param>
    public void AddPressureSwitch(PressureSwitch currentSwitch)
    {
        if (!currentSwitchesOpen.Contains(currentSwitch))
        {
            currentSwitchesOpen.Add(currentSwitch);
        }

        TryOpen();
    }

    /// <summary>
    /// Removes the given pressure switch from the list of active switches.
    /// Checks if the door needs to be closed.
    /// </summary>
    /// <param name="currentSwitch">The pressure switch to remove.</param>
    public void RemovePressureSwitch(PressureSwitch currentSwitch)
    {
        if (currentSwitchesOpen.Contains(currentSwitch))
        {
            currentSwitchesOpen.Remove(currentSwitch);
        }

        TryOpen();
    }

    /// <summary>
    /// Tries to open or close the door based on the number of active switches.
    /// </summary>
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

    /// <summary>
    /// Opens the door by triggering the animation.
    /// </summary>
    private void OpenDoor()
    {
        if (!isDoorOpen)
        {
            currentObject.SetActive(true);
        }

        isDoorOpen = true;
    }

    /// <summary>
    /// Closes the door by triggering the animation.
    /// </summary>
    private void CloseDoor()
    {
        if (isDoorOpen)
        {
            currentObject.SetActive(false);
        }

        isDoorOpen = false;
    }
}