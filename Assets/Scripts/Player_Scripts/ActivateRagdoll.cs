using UnityEngine;

public class ActivateRagdoll : MonoBehaviour
{
    /// <summary>
    /// Activates the ragdoll state by turning off balance and pose using DriveOff.
    /// </summary>
    /// <param name="balanced">The current balance state of the character.</param>
    /// <param name="isRagdoll">Indicates if the character is currently in ragdoll state.</param>
    /// <param name="reachRightAxisUsed">Indicates if the character's right axis is being used.</param>
    /// <param name="reachLeftAxisUsed">Indicates if the character's left axis is being used.</param>
    /// <param name="playerPart0">The root player part GameObject.</param>
    /// <param name="playerParts">An array of player part GameObjects.</param>
    /// <param name="driveOff">The JointDrive used for deactivating the ragdoll state.</param>
    public void RagdollActivator(ref bool balanced,
        ref bool isRagdoll,
        bool reachRightAxisUsed,
        bool reachLeftAxisUsed,
        ConfigurableJoint[] playerParts,
        JointDrive DriveOff)
    {
        // To start being a ragdoll, the balance and pose are set OFF, using DriveOff.
        isRagdoll = true;
        balanced = false;

        playerParts[0].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[0].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;

        for (int i = 2; i < playerParts.Length; i++)
        {
            playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        if (!reachRightAxisUsed)
        {
            for (int i = 3; i <= 4; i++)
            {
                playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            }
        }

        if (!reachLeftAxisUsed)
        {
            for (int i = 5; i <= 6; i++)
            {
                playerParts[i].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                playerParts[i].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            }
        }
    }
}