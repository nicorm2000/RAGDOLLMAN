using UnityEngine;

public class DeactivateRagdoll : MonoBehaviour
{
    /// <summary>
    /// Deactivates player raagdoll, which controls balance.
    /// </summary>
    public void RagdollDeactivator(bool balanced, bool isRagdoll, bool reachRightAxisUsed, bool reachLeftAxisUsed, bool resetPose,GameObject playerPart0, GameObject playerPart2, GameObject playerPart3, GameObject playerPart4, GameObject playerPart5, GameObject playerPart6, GameObject playerPart7, GameObject playerPart8, GameObject playerPart9, GameObject playerPart10, GameObject playerPart11, GameObject playerPart12, JointDrive balanceOn, JointDrive poseOn)
    {
        //To stop being a raggdoll the balance and pose are set ON.

        isRagdoll = false;
        balanced = true;

        //Root
        playerPart0.GetComponent<ConfigurableJoint>().angularXDrive = balanceOn;
        playerPart0.GetComponent<ConfigurableJoint>().angularYZDrive = balanceOn;
        //Head
        playerPart2.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart2.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;

        //Arms
        if (!reachRightAxisUsed)
        {
            playerPart3.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
            playerPart3.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
            playerPart4.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
            playerPart4.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        }

        if (!reachLeftAxisUsed)
        {
            playerPart5.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
            playerPart5.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
            playerPart6.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
            playerPart6.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        }

        //Legs
        playerPart7.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart7.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        playerPart8.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart8.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        playerPart9.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart9.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        playerPart10.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart10.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        playerPart11.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart11.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;
        playerPart12.GetComponent<ConfigurableJoint>().angularXDrive = poseOn;
        playerPart12.GetComponent<ConfigurableJoint>().angularYZDrive = poseOn;

        resetPose = true;
    }
}