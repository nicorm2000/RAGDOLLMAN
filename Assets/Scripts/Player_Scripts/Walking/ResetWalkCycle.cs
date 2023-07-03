using UnityEngine;

/// <summary>
/// Resets the stepRight, stepLeft, alert_Leg_Right, and alert_Leg_Left booleans and the step_R_timer and step_L_timer variables to 0 when not moving.
/// </summary>
public class ResetWalkCycle : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// Resets the stepRight, stepLeft, alert_Leg_Right, and alert_Leg_Left booleans and the step_R_timer and step_L_timer variables to 0 when not moving.
    /// </summary>
    public void WalkCycleReset()
    {
        //Reset variables when not moving
        if (!playerController.walkForward && !playerController.walkBackward)
        {
            playerController.stepRight = false;

            playerController.stepLeft = false;

            playerController.stepTimerRight = 0;

            playerController.stepTimerLeft = 0;

            playerController.alertLegRight = false;

            playerController.alertLegLeft = false;
        }
    }
}