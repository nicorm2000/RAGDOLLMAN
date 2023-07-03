using UnityEngine;

/// <summary>
/// Deactivates the ragdoll state and restores the character's pose and balance.
/// </summary>
public class DeactivateRagdoll : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// Deactivates the ragdoll state and restores the character's pose and balance.
    /// </summary>
    public void RagdollDeactivator()
    {
        // To stop being a ragdoll, the balance and pose are set ON.
        playerController.isRagdoll = false;
        playerController.balanced = true;

        playerController.playerParts[0].angularXDrive = playerController.BalanceOn;

        playerController.playerParts[0].angularYZDrive = playerController.BalanceOn;

        for (int i = 2; i < playerController.playerParts.Length; i++)
        {
            playerController.playerParts[i].angularXDrive = playerController.PoseOn;

            playerController.playerParts[i].angularYZDrive = playerController.PoseOn;
        }

        if (!playerController.reachRightAxisUsed)
        {
            for (int i = 3; i <= 4; i++)
            {
                playerController.playerParts[i].angularXDrive = playerController.PoseOn;

                playerController.playerParts[i].angularYZDrive = playerController.PoseOn;
            }
        }

        if (!playerController.reachLeftAxisUsed)
        {
            for (int i = 5; i <= 6; i++)
            {
                playerController.playerParts[i].angularXDrive = playerController.PoseOn;

                playerController.playerParts[i].angularYZDrive = playerController.PoseOn;
            }
        }

        playerController.resetPose = true;
    }
}