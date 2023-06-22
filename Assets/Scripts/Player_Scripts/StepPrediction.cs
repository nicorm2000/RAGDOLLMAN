using UnityEngine;

public class StepPrediction : MonoBehaviour
{
    [SerializeField] ResetWalkCycle resetWalkCycle;

    /// <summary>
    /// Predicts the player's next step and sets the values of walkForward and walkBackward accordingly. If the player is off-balance, sets walkBackward to true if the COMP object is behind both feet, and sets walkForward to true if the COMP object is in front of both feet. If isKeyDown is false, walkForward and walkBackward are set to false.
    /// </summary>
    /// <param name="walkForward">A reference to a boolean representing whether or not the player is walking forward.</param>
    /// <param name="walkBackward">A reference to a boolean representing whether or not the player is walking backward.</param>
    /// <param name="COMP">The position of the player's center of mass.</param>
    /// <param name="playerParts">An array of GameObjects representing the parts of the player's body.</param>
    /// <param name="isKeyDown">A boolean representing whether or not a key is being pressed.</param>
    /// <param name="stepRight">A reference to a boolean representing whether or not the right leg is stepping.</param>
    /// <param name="stepLeft">A reference to a boolean representing whether or not the left leg is stepping.</param>
    /// <param name="alert_Leg_Right">A reference to a boolean representing whether or not the right leg is in an alert state.</param>
    /// <param name="alert_Leg_Left">A reference to a boolean representing whether or not the left leg is in an alert state.</param>
    /// <param name="step_R_timer">A reference to a float representing the time elapsed since the right leg stepped.</param>
    /// <param name="step_L_timer">A reference to a float representing the time elapsed since the left leg stepped.</param>
    public void PredictNextStep(ref bool walkForward, 
        ref bool walkBackward, 
        Transform COMP, 
        GameObject[] playerParts, 
        bool isKeyDown,
        ref bool stepRight, 
        ref bool stepLeft, 
        ref bool alert_Leg_Right, 
        ref bool alert_Leg_Left, 
        ref float step_R_timer, 
        ref float step_L_timer)
    {
        bool notWalking = !walkForward && !walkBackward;

        //Reset variables when balanced
        if (notWalking)
        {
            resetWalkCycle.WalkCycleReset(walkForward, 
                walkBackward, 
                ref stepRight, 
                ref stepLeft, 
                ref alert_Leg_Right, 
                ref alert_Leg_Left, 
                ref step_R_timer, 
                ref step_L_timer);
        }

        //Check direction to walk when off balance
        //Walk backward
        if (COMP.position.z < playerParts[11].transform.position.z && COMP.position.z < playerParts[12].transform.position.z)
        {
            walkBackward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                walkBackward = false;
            }
        }

        //Walk forward
        if (COMP.position.z > playerParts[11].transform.position.z && COMP.position.z > playerParts[12].transform.position.z)
        {
            walkForward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                walkForward = false;
            }
        }
    }
}