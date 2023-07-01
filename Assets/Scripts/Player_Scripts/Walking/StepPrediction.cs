using UnityEngine;

public class StepPrediction : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Reset Walk Cycle Dependancies")]
    [SerializeField] private ResetWalkCycle resetWalkCycle;

    /// <summary>
    /// Predicts the player's next step and sets the values of walkForward and walkBackward accordingly. If the player is off-balance, sets walkBackward to true if the COMP object is behind both feet, and sets walkForward to true if the COMP object is in front of both feet. If isKeyDown is false, walkForward and walkBackward are set to false.
    /// </summary>
    public void PredictNextStep()
    {
        bool notWalking = !playerController.walkForward && !playerController.walkBackward;

        //Reset variables when balanced
        if (notWalking)
        {
            resetWalkCycle.WalkCycleReset();
        }

        //Check direction to walk when off balance
        //Walk backward
        if (playerController.COMP.position.z < playerController.playerParts[11].gameObject.transform.position.z && playerController.COMP.position.z < playerController.playerParts[12].transform.position.z)
        {
            playerController.walkBackward = true;
        }
        else
        {
            if (!playerController.isKeyDown)
            {
                playerController.walkBackward = false;
            }
        }

        //Walk forward
        if (playerController.COMP.position.z > playerController.playerParts[11].transform.position.z && playerController.COMP.position.z > playerController.playerParts[12].transform.position.z)
        {
            playerController.walkForward = true;
        }
        else
        {
            if (!playerController.isKeyDown)
            {
                playerController.walkForward = false;
            }
        }
    }
}