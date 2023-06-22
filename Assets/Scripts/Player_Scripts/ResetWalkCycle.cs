using UnityEngine;

public class ResetWalkCycle : MonoBehaviour
{
    /// <summary>
    /// Resets the stepRight, stepLeft, alert_Leg_Right, and alert_Leg_Left booleans and the step_R_timer and step_L_timer variables to 0 when not moving.
    /// </summary>
    /// <param name="walkForward">A boolean representing whether or not the player is walking forward.</param>
    /// <param name="walkBackward">A boolean representing whether or not the player is walking backward.</param>
    /// <param name="stepRight">A reference to a boolean representing whether or not the right leg is stepping.</param>
    /// <param name="stepLeft">A reference to a boolean representing whether or not the left leg is stepping.</param>
    /// <param name="alert_Leg_Right">A reference to a boolean representing whether or not the right leg is in an alert state.</param>
    /// <param name="alert_Leg_Left">A reference to a boolean representing whether or not the left leg is in an alert state.</param>
    /// <param name="step_R_timer">A reference to a float representing the time elapsed since the right leg stepped.</param>
    /// <param name="step_L_timer">A reference to a float representing the time elapsed since the left leg stepped.</param>
    public void WalkCycleReset(bool walkForward, 
        bool walkBackward, 
        ref bool stepRight, 
        ref bool stepLeft, 
        ref bool alert_Leg_Right, 
        ref bool alert_Leg_Left, 
        ref float step_R_timer, 
        ref float step_L_timer)
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