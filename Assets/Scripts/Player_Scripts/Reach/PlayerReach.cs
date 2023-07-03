using UnityEngine;

/// <summary>
/// Controls the player's reach with both hands, including handling joint rotations and stiffness adjustments.
/// </summary>
public class PlayerReach : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Player Reach Clamp Values")]
    [SerializeField] private float clampYValue = -0.9f;
    [SerializeField] private float clampZValue = 0.9f;
    [SerializeField] private float clampYValueIfIsReaching = -1.2f;
    [SerializeField] private float clampZValueIfIsReaching = 1.2f;

    public bool IsReachingLeft { get; private set; }
    public bool IsReachingRight { get; private set; }

    private float moveBodyValue;

    /// <summary>
    /// Handles the player's reach with both hands.
    /// </summary>
    public void ReachHands()
    {
        //Simulates body bending by allowing the player to move the mouse up and down to control the rotation of a ConfigurableJoint component attached to playerParts[1]
        float newYAxis = playerController.MouseYAxisBody + (moveBodyValue / playerController.reachSensitivity);

        newYAxis = Mathf.Clamp(newYAxis, clampYValue, clampZValue);

        playerController.MouseYAxisBody = newYAxis;

        playerController.playerParts[1].targetRotation = new Quaternion(playerController.MouseYAxisBody, 0, 0, 1);

        //Set the targetRotation of the ConfigurableJoint attached to playerParts[1] to a new quaternion that represents the rotation of the torso. 
        playerController.playerParts[1].targetRotation = new Quaternion(playerController.MouseYAxisBody, 0, 0, 1);

        //Handle player's left reach
        if (IsReachingLeft)
        {
            if (!playerController.reachLeftAxisUsed)
            {
                AdjustJointStrengths(playerController, 5, 6, 1);

                playerController.reachLeftAxisUsed = true;
            }

            float newYAxisArms = playerController.MouseYAxisArms + (moveBodyValue / playerController.reachSensitivity);

            newYAxisArms = Mathf.Clamp(newYAxisArms, clampYValueIfIsReaching, clampZValueIfIsReaching);
            
            playerController.MouseYAxisArms = newYAxisArms;

            playerController.playerParts[5].targetRotation = new Quaternion(-0.58f - newYAxisArms, -0.88f - newYAxisArms, -0.8f, 1);
        }

        if (!IsReachingLeft && playerController.reachLeftAxisUsed)
        {
            if (playerController.balanced)
            {
                SetJointStiffness(playerController, true, 5, 6, 1);
            }
            else if (!playerController.balanced)
            {
                SetJointStiffness(playerController, false, 5, 6, 1);
            }

            playerController.resetPose = true;

            playerController.reachLeftAxisUsed = false;
        }

        //Handle player's right reach
        if (IsReachingRight)
        {
            if (!playerController.reachRightAxisUsed)
            {
                AdjustJointStrengths(playerController, 3, 4, 1);

                playerController.reachRightAxisUsed = true;
            }

            float newYAxisArms = playerController.MouseYAxisArms + (moveBodyValue / playerController.reachSensitivity);

            newYAxisArms = Mathf.Clamp(newYAxisArms, clampYValueIfIsReaching, clampZValueIfIsReaching);

            playerController.MouseYAxisArms = newYAxisArms;

            playerController.playerParts[3].targetRotation = new Quaternion(0.58f - newYAxisArms, -0.88f - newYAxisArms, 0.8f, 1);
        }

        if (!IsReachingRight && playerController.reachRightAxisUsed)
        {
            //Sets the right reach input is not being used (specified by the reachRight variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to playerParts[3], playerParts[4], and playerParts[1]
            if (playerController.reachRightAxisUsed)
            {
                if (playerController.balanced)
                {
                    SetJointStiffness(playerController, true, 3, 4, 1);
                }
                else if (!playerController.balanced)
                {
                    SetJointStiffness(playerController, false, 3, 4, 1);
                }

                playerController.resetPose = true;

                playerController.reachRightAxisUsed = false;
            }
        }
    }

    /// <summary>
    /// Sets the state of left hand reaching.
    /// </summary>
    /// <param name="currentState">The current state of left hand reaching.</param>
    public void HandReachLeft(bool currentState) => IsReachingLeft = currentState;

    /// <summary>
    /// Sets the state of right hand reaching.
    /// </summary>
    /// <param name="currentState">The current state of right hand reaching.</param>
    public void HandReachRight(bool currentState) => IsReachingRight = currentState;

    /// <summary>
    /// Moves the body with the specified value.
    /// </summary>
    /// <param name="currentValue">The value used to move the body.</param>
    public void MoveBody(float currentValue) => moveBodyValue = currentValue;

    /// <summary>
    /// Adjusts the joint strengths of the specified player parts.
    /// </summary>
    /// <param name="player">The PlayerController instance.</param>
    /// <param name="firstPart">The index of the first player part.</param>
    /// <param name="secondPart">The index of the second player part.</param>
    /// <param name="thirdPart">The index of the third player part.</param>
    private void AdjustJointStrengths(PlayerController player, int firstPart, int secondPart, int thirdPart)
    {
        player.playerParts[firstPart].angularXDrive = player.ReachStiffness;
        player.playerParts[firstPart].angularYZDrive = player.ReachStiffness;
        player.playerParts[secondPart].angularXDrive = player.ReachStiffness;
        player.playerParts[secondPart].angularYZDrive = player.ReachStiffness;

        player.playerParts[thirdPart].angularXDrive = player.CoreStiffness;
        player.playerParts[thirdPart].angularYZDrive = player.CoreStiffness;
    }

    /// <summary>
    /// Sets the joint stiffness of the specified player parts.
    /// </summary>
    /// <param name="player">The PlayerController instance.</param>
    /// <param name="stiffness">A boolean indicating whether to set the stiffness to "PoseOn" or "DriveOff".</param>
    /// <param name="firstPart">The index of the first player part.</param>
    /// <param name="secondPart">The index of the second player part.</param>
    /// <param name="thirdPart">The index of the third player part.</param>
    private void SetJointStiffness(PlayerController player, bool stiffness, int firstPart, int secondPart, int thirdPart)
    {
        if (stiffness)
        {
            player.playerParts[firstPart].angularXDrive = player.PoseOn;
            player.playerParts[firstPart].angularYZDrive = player.PoseOn;
            player.playerParts[secondPart].angularXDrive = player.PoseOn;
            player.playerParts[secondPart].angularYZDrive = player.PoseOn;
            player.playerParts[thirdPart].angularXDrive = player.PoseOn;
            player.playerParts[thirdPart].angularYZDrive = player.PoseOn;
        }
        else
        {
            player.playerParts[firstPart].angularXDrive = player.DriveOff;
            player.playerParts[firstPart].angularYZDrive = player.DriveOff;
            player.playerParts[secondPart].angularXDrive = player.DriveOff;
            player.playerParts[secondPart].angularYZDrive = player.DriveOff;
        }
    }
}