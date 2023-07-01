using UnityEngine;

public class ResetPlayerPose : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

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
    public void PlayerPoseReset()
    {
        if (playerController.resetPose && !playerController.jumping)
        {
            playerController.playerParts[1].GetComponent<ConfigurableJoint>().targetRotation = playerController.BodyTarget;
            playerController.playerParts[3].GetComponent<ConfigurableJoint>().targetRotation = playerController.UpperRightArmTarget;
            playerController.playerParts[4].GetComponent<ConfigurableJoint>().targetRotation = playerController.LowerRightArmTarget;
            playerController.playerParts[5].GetComponent<ConfigurableJoint>().targetRotation = playerController.UpperLeftArmTarget;
            playerController.playerParts[6].GetComponent<ConfigurableJoint>().targetRotation = playerController.LowerLeftArmTarget;

            playerController.MouseYAxisArms = 0;

            playerController.resetPose = false;
        }
    }
}