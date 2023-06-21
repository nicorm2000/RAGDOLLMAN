using UnityEngine;

public class ActivateRagdoll : MonoBehaviour
{
    /// <summary>
    /// Activates player raagdoll, which controls balance.
    /// </summary>
    public void RagdollActivator(bool balanced, bool isRagdoll, bool reachRightAxisUsed, bool reachLeftAxisUsed, GameObject playerPart0, GameObject playerPart2, GameObject playerPart3, GameObject playerPart4, GameObject playerPart5, GameObject playerPart6, GameObject playerPart7, GameObject playerPart8, GameObject playerPart9, GameObject playerPart10, GameObject playerPart11, GameObject playerPart12, JointDrive driveOff)
    {
        //To start being a raggdoll the balance and pose are set OFF, by using DriveOff.

        isRagdoll = true;
        balanced = false;

        //Root
        playerPart0.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart0.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        //Head
        playerPart2.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart2.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;

        //Arms
        if (!reachRightAxisUsed)
        {
            playerPart3.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
            playerPart3.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
            playerPart4.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
            playerPart4.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        }

        if (!reachLeftAxisUsed)
        {
            playerPart5.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
            playerPart5.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
            playerPart6.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
            playerPart6.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        }

        //Legs
        playerPart7.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart7.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        playerPart8.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart8.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        playerPart9.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart9.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        playerPart10.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart10.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        playerPart11.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart11.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
        playerPart12.GetComponent<ConfigurableJoint>().angularXDrive = driveOff;
        playerPart12.GetComponent<ConfigurableJoint>().angularYZDrive = driveOff;
    }
}