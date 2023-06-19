using UnityEngine;

public class ResetWalkCycle : MonoBehaviour
{
    /// <summary>
    /// Resets the walk cycle variables when the character is not moving.
    /// </summary>
    public void WalkCycleReset(bool walkForward, bool walkBackward, bool stepRight, bool stepLeft, bool alert_Leg_Right, bool alert_Leg_Left, float step_R_timer, float step_L_timer)
    {
        //Reset variables when not moving
        if (!walkForward && !walkBackward)
        {
            stepRight = false;
            stepLeft = false;
            step_R_timer = 0;
            step_L_timer = 0;
            alert_Leg_Right = false;
            alert_Leg_Left = false;
        }
    }
}