using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    [Header("Switch Configuration")]
    [SerializeField] private Door currentDoor;

    [SerializeField] private PressureSwitchAnimator switchAnimator;

    [SerializeField] private LayerMask playerLayer;

    /// <summary>
    /// Called when a collider stays within the switch trigger.
    /// Adds the pressure switch to the door and triggers animation.
    /// </summary>
    /// <param name="other">The collider staying within the switch trigger.</param>
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentDoor.AddPressureSwitch(this);
            switchAnimator.SetSwitchState(true);
        }
    }

    /// <summary>
    /// Called when a collider exits the switch trigger.
    /// Removes the pressure switch from the door and triggers animation.
    /// </summary>
    /// <param name="other">The collider exiting the switch trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentDoor.RemovePressureSwitch(this);
            switchAnimator.SetSwitchState(false);
        }
    }
}