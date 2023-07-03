using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages input actions and controls the player character based on the input.
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Player Controller Configuration")]
    [SerializeField] CameraController cameraController;
    [SerializeField] float xAxisValue = 100f;
    [SerializeField] float yAxisValue = 100f;

    [Header("Player Controller Dependencies")]
    [SerializeField] GetUpByJumping getUpController;

    [Header("Player Controller Configuration")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] float moveBodyValue = -100f;

    [Header("Player Reach Dependencies")]
    [SerializeField] PlayerReach playerReach;

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
            cameraController.currentX += context.ReadValue<Vector2>().x * cameraController.rotateSpeed * xAxisValue * Time.deltaTime;
            cameraController.currentY += context.ReadValue<Vector2>().y * cameraController.rotateSpeed * yAxisValue * Time.deltaTime;
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

    /// <summary>
    /// Handles the left hand reach input action.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnReachLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Started)
        {
            playerReach.HandReachLeft(context.phase == InputActionPhase.Started);
        }
    }

    /// <summary>
    /// Handles the right hand reach input action.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnReachRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Started)
        {
            playerReach.HandReachRight(context.phase == InputActionPhase.Started);
        }
    }

    /// <summary>
    /// Handles the mouse movement of the player's body.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMoveBodyMouse(InputAction.CallbackContext context)
    {
        float movementBodyInput = context.phase == InputActionPhase.Canceled ? 0.0f: context.ReadValue<float>();
        playerReach.MoveBody(movementBodyInput);
    }

    /// <summary>
    /// Handles the controller movement of the player's body.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMoveBodyController(InputAction.CallbackContext context)
    {
        float movementBodyInput = context.phase == InputActionPhase.Canceled ? 0.0f : context.ReadValue<float>() * moveBodyValue;
        playerReach.MoveBody(movementBodyInput);
    }
}