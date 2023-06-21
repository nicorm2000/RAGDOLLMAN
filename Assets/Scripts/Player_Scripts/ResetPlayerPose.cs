using UnityEngine;

public class ResetPlayerPose : MonoBehaviour
{
    /// <summary>
    /// Resets the pose of the player if the resetPose boolean is true and the player is not jumping.
    /// </summary>
    /// <param name="resetPose">A boolean representing whether or not the player's pose should be reset.</param>
    /// <param name="jumping">A boolean representing whether or not the player is jumping.</param>
    /// <param name="MouseYAxisArms">The Y-axis rotation for the player's arms, used to reset the pose.</param>
    /// <param name="playerParts">An array of GameObjects representing the parts of the player's body.</param>
    /// <param name="BodyTarget">A Quaternion representing the target rotation for the player's body.</param>
    /// <param name="UpperRightArmTarget">A Quaternion representing the target rotation for the player's upper right arm.</param>
    /// <param name="LowerRightArmTarget">A Quaternion representing the target rotation for the player's lower right arm.</param>
    /// <param name="UpperLeftArmTarget">A Quaternion representing the target rotation for the player's upper left arm.</param>
    /// <param name="LowerLeftArmTarget">A Quaternion representing the target rotation for the player's lower left arm.</param>
    public void PlayerPoseReset(bool resetPose, bool jumping, float MouseYAxisArms, GameObject[] playerParts, Quaternion BodyTarget, Quaternion UpperRightArmTarget, Quaternion LowerRightArmTarget, Quaternion UpperLeftArmTarget, Quaternion LowerLeftArmTarget)
    {
        if (resetPose && !jumping)
        {
            playerParts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
            playerParts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
            playerParts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
            playerParts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
            playerParts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

            MouseYAxisArms = 0;

            resetPose = false;
        }
    }
}