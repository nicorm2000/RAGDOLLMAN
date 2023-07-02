using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] CameraController cameraController;

    [Header("Player Controller Dependencies")]
    [SerializeField] GetUpByJumping getUpController;

    [Header("Player Controller Dependencies")]
    [SerializeField] PlayerMovement playerMovement;

    /// <summary>
    /// Callback function for mouse camera movement input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMouseCameraMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraController.currentX += context.ReadValue<Vector2>().x * cameraController.rotateSpeed * Time.deltaTime;
            cameraController.currentY += context.ReadValue<Vector2>().y * cameraController.rotateSpeed * Time.deltaTime;
        }
        else
        {
            cameraController.currentX += 0;
            cameraController.currentY += 0;
        }
    }

    /// <summary>
    /// Callback function for controller camera movement input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnControllerCameraMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraController.currentX += context.ReadValue<Vector2>().x * cameraController.rotateSpeed * 10 * Time.deltaTime;
            cameraController.currentY -= context.ReadValue<Vector2>().y * cameraController.rotateSpeed * 10 * Time.deltaTime;
        }
        else
        {
            cameraController.currentX += 0;
            cameraController.currentY += 0;
        }
    }

    /// <summary>
    /// Handles the jump action based on the input context.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!getUpController.playerController.jumpAxisUsed)
            {
                if (getUpController.playerController.balanced && !getUpController.playerController.inAir)
                {
                    getUpController.playerController.jumping = true;
                }
                else if (!getUpController.playerController.balanced)
                {
                    getUpController.deactivateRagdoll.RagdollDeactivator();
                }
            }

            getUpController.playerController.jumpAxisUsed = true;
        }
        else if (context.canceled)
        {
            getUpController.playerController.jumpAxisUsed = false;
        }
    }

    /// <summary>
    /// Handles the movement input action.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.phase == InputActionPhase.Canceled ? Vector2.zero : context.ReadValue<Vector2>();
        playerMovement.SetMovement(movementInput);
    }
}