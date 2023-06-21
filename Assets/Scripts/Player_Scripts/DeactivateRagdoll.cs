using UnityEngine;

public class DeactivateRagdoll : MonoBehaviour
{
    /// <summary>
    /// Deactivates the ragdoll state and restores the character's pose and balance.
    /// </summary>
    /// <param name="balanced">The current balance state of the character.</param>
    /// <param name="isRagdoll">Indicates if the character is currently in ragdoll state.</param>
    /// <param name="reachRightAxisUsed">Indicates if the character's right axis is being used.</param>
    /// <param name="reachLeftAxisUsed">Indicates if the character's left axis is being used.</param>
    /// <param name="resetPose">Indicates if the character's pose should be reset.</param>
    /// <param name="playerParts">An array of GameObjects representing different parts of the player.</param>
    /// <param name="BalanceOn">The JointDrive used for balancing.</param>
    /// <param name="PoseOn">The JointDrive used for restoring the pose.</param>
    private void RagdollDeactivator(bool balanced, 
        bool isRagdoll, 
        bool reachRightAxisUsed, 
        bool reachLeftAxisUsed, 
        bool resetPose, 
        GameObject[] playerParts, 
        JointDrive BalanceOn, 
        JointDrive PoseOn)
    {
        // To stop being a ragdoll, the balance and pose are set ON.
        isRagdoll = false;
        balanced = true;

        playerParts[0].GetComponent<ConfigurableJoint>().angularXDrive = BalanceOn;

        playerParts[0].GetComponent<ConfigurableJoint>().angularYZDrive = BalanceOn;

        for (int i = 2; i < playerParts.Length; i++)
        {
            playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;

            playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        if (!reachRightAxisUsed)
        {
            for (int i = 3; i <= 4; i++)
            {
                playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;

                playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            }
        }

        if (!reachLeftAxisUsed)
        {
            for (int i = 5; i <= 6; i++)
            {
                playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;

                playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            }
        }

        resetPose = true;
    }
}