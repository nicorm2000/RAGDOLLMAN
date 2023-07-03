using UnityEngine;

/// <summary>
/// Resets the pose of the player if the resetPose boolean is true and the player is not jumping.
/// </summary>
public class ResetPlayerPose : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// Resets the pose of the player if the resetPose boolean is true and the player is not jumping.
    /// </summary>
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