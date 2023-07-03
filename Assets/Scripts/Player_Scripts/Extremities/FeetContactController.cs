using UnityEngine;

public class FeetContactController : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Player Landed Dependencies")]
    [SerializeField] private PlayerLanded playerLanded;

    [Header("Layer to Check")]
    [SerializeField] private string layerToCheck = "Ground";

    /// <summary>
    /// Called when the GameObject collides with another GameObject. Checks if the player is not jumping and is in the air, and if so, checks if the collision was with an object on the "Ground" layer. If both conditions are met, calls the PlayerLanded method of the playerController object.
    /// </summary>
    /// <param name="col">The Collision object representing the collision that occurred.</param>
    void OnCollisionEnter(Collision col)
    {
        if (!playerController.isJumping && playerController.inAir)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(layerToCheck))
            {
                playerLanded.PlayerHasLanded(ref playerController.inAir, playerController.isJumping, playerController.jumping, ref playerController.resetPose);
            }
        }
    }
}