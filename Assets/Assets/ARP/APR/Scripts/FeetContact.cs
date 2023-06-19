using UnityEngine;

public class FeetContact : MonoBehaviour
{
    public PlayerController playerController;

    /// <summary>
    /// Called when the GameObject collides with another GameObject. Checks if the player is not jumping and is in the air, and if so, checks if the collision was with an object on the "Ground" layer. If both conditions are met, calls the PlayerLanded method of the playerController object.
    /// </summary>
    /// <param name="col">The Collision object representing the collision that occurred.</param>
    void OnCollisionEnter(Collision col)
    {
        if (!playerController.isJumping && playerController.inAir)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                playerController.PlayerLanded();
            }
        }
    }
}