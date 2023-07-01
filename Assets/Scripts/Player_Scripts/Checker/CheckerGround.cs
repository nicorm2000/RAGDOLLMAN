using UnityEngine;

public class CheckerGround : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// Determines if the character is in an idle state.
    /// </summary>
    /// <param name="inAir">Indicates if the character is in the air.</param>
    /// <param name="isJumping">Indicates if the character is currently jumping.</param>
    /// <param name="reachRightAxisUsed">Indicates if the character's right axis is being used.</param>
    /// <param name="reachLeftAxisUsed">Indicates if the character's left axis is being used.</param>
    /// <returns>Returns true if the character is idle; otherwise, returns false.</returns>
    private bool IsIdle(bool inAir, 
        bool isJumping, 
        bool reachRightAxisUsed, 
        bool reachLeftAxisUsed)
    {
        return !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed;
    }

    /// <summary>
    /// Checks if the character is grounded and adjusts balance accordingly.
    /// </summary>
    public void GroundChecker()
    {
        Transform playerTransform = playerController.playerParts[0].transform;
        Rigidbody playerRigidbody = playerController.playerParts[0].GetComponent<Rigidbody>();

        Ray ray = new Ray(playerTransform.position, -playerTransform.up);
        RaycastHit hit;

        LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out hit, playerController.balanceHeight, groundLayer) && IsIdle(playerController.inAir,
            playerController.isJumping,
            playerController.reachRightAxisUsed,
            playerController.reachLeftAxisUsed))
        {
            if (!playerController.balanced && playerRigidbody.velocity.magnitude < 1f && playerController.autoGetUpWhenPossible)
            {
                playerController.balanced = true;
            }
        }
        else if (!Physics.Raycast(ray, out hit, playerController.balanceHeight, groundLayer))
        {
            if (playerController.balanced)
            {
                playerController.balanced = false;
            }
        }
    }
}