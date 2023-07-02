using UnityEngine;
using UnityEngine.InputSystem;

public class GetUpByJumping : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Ragdoll Deactivator Dependancies")]
    [SerializeField] private DeactivateRagdoll deactivateRagdoll;

    /// <summary>
    /// Handles the player character's getting up and jumping actions.
    /// </summary>
    public void PlayerGetUpJumping()
    {
        //Calculates the jump force by multiplying the transform.up vector of the Rigidbody attached to playerParts[0] by jumpForce,
        //and sets the x and z values of the vector to the respective values of the current velocity of the Rigidbody.
        if (playerController.jumping)
        {
            playerController.isJumping = true;

            var v3 = playerController.playerParts[0].GetComponent<Rigidbody>().transform.up * playerController.jumpForce;

            v3.x = playerController.playerParts[0].GetComponent<Rigidbody>().velocity.x;

            v3.z = playerController.playerParts[0].GetComponent<Rigidbody>().velocity.z;

            playerController.playerParts[0].GetComponent<Rigidbody>().velocity = v3;
        }

        if (playerController.isJumping)
        {
            playerController.timer = playerController.timer + Time.fixedDeltaTime;

            if (playerController.timer > 0.2f)
            {
                playerController.timer = 0.0f;

                playerController.jumping = false;

                playerController.isJumping = false;

                playerController.inAir = true;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!playerController.jumpAxisUsed)
            {
                if (playerController.balanced && !playerController.inAir)
                {
                    playerController.jumping = true;
                }
                else if (!playerController.balanced)
                {
                    deactivateRagdoll.RagdollDeactivator();
                }
            }

            playerController.jumpAxisUsed = true;
        }
        else if (context.canceled)
        {
            playerController.jumpAxisUsed = false;
        }
    }
}