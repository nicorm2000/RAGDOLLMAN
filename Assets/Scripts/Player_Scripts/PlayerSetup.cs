using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [Header("Player Controller Dependancies")]
    [SerializeField] PlayerController playerController;

    /// <summary>
    /// Sets up the Player and its components(camera, joint drives, body parts target rotations to each body part).
    /// </summary>
    public void SetupPlayer()
    {
        playerController.cam = Camera.main;

        //Setup joint drives

        //playerController.BalanceOn: sets the position spring, position damper, and maximum force for balancing.
        playerController.BalanceOn = new JointDrive();

        playerController.BalanceOn.positionSpring = playerController.balanceStrength;

        playerController.BalanceOn.positionDamper = 0;

        playerController.BalanceOn.maximumForce = Mathf.Infinity;

        //playerController.PoseOn: sets the position spring, position damper, and maximum force for poses.
        playerController.PoseOn = new JointDrive();

        playerController.PoseOn.positionSpring = playerController.limbStrength;

        playerController.PoseOn.positionDamper = 0;

        playerController.PoseOn.maximumForce = Mathf.Infinity;

        //playerController.CoreStiffness: sets the position spring, position damper, and maximum force for core stiffness.
        playerController.CoreStiffness = new JointDrive();

        playerController.CoreStiffness.positionSpring = playerController.coreStrength;

        playerController.CoreStiffness.positionDamper = 0;

        playerController.CoreStiffness.maximumForce = Mathf.Infinity;

        //playerController.ReachStiffness: sets the position spring, position damper, and maximum force for reaching.
        playerController.ReachStiffness = new JointDrive();

        playerController.ReachStiffness.positionSpring = playerController.armReachStiffness;

        playerController.ReachStiffness.positionDamper = 0;

        playerController.ReachStiffness.maximumForce = Mathf.Infinity;

        //playerController.DriveOff: sets the position spring, position damper, and maximum force for driving off.
        playerController.DriveOff = new JointDrive();

        playerController.DriveOff.positionSpring = 25;

        playerController.DriveOff.positionDamper = 0;

        playerController.DriveOff.maximumForce = Mathf.Infinity;

        //Setup/reroute active ragdoll parts to array
        playerController.playerParts = new ConfigurableJoint[] { playerController.Root, 
            playerController.Body, 
            playerController.Head, 
            playerController.UpperRightArm, 
            playerController.LowerRightArm, 
            playerController.UpperLeftArm, 
            playerController.LowerLeftArm, 
            playerController.UpperRightLeg, 
            playerController.LowerRightLeg, 
            playerController.UpperLeftLeg, 
            playerController.LowerLeftLeg, 
            playerController.RightFoot, 
            playerController.LeftFoot };

        //Setup original pose for joint drives
        //Initializes an array of type Quaternion (because we are workingg with rotations) with a length of 10 called targets.
        //It then uses a loop to populate each element in the array with the target rotation of each joint in the player's body,
        //taken from the playerController.playerParts GameObject array.
        //Finally, it assigns the target rotations stored in the targets array to the relevant playerController variables.
        Quaternion[] targets = new Quaternion[10];

        for (int i = 1; i <= 10; i++)
        {
            targets[i - 1] = playerController.playerParts[i].targetRotation;
        }

        playerController.BodyTarget = targets[0];
        playerController.HeadTarget = targets[1];
        playerController.UpperRightArmTarget = targets[2];
        playerController.LowerRightArmTarget = targets[3];
        playerController.UpperLeftArmTarget = targets[4];
        playerController.LowerLeftArmTarget = targets[5];
        playerController.UpperRightLegTarget = targets[6];
        playerController.LowerRightLegTarget = targets[7];
        playerController.UpperLeftLegTarget = targets[8];
        playerController.LowerLeftLegTarget = targets[9];
    }
}
