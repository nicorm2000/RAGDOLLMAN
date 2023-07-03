using UnityEngine;

/// <summary>
/// Controls the player's movement based on input and manages the player's state.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    private Vector2 movementValue = Vector2.zero;

    private PlayerState currentState;

    public enum PlayerState
    {
        Balanced,
        NotBalanced
    }

    /// <summary>
    /// Sets the player's movement based on the provided normalized movement input.
    /// </summary>
    /// <param name="normalizedMovement">The normalized movement input.</param>
    public void SetMovement(Vector2 normalizedMovement) => movementValue = normalizedMovement;

    /// <summary>
    /// Initializes the Finite State Machine (FSM) with the starting state.
    /// </summary>
    private void Start()
    {
        // Initialize the FSM with the starting state
        currentState = PlayerState.NotBalanced;
    }

    /// <summary>
    /// Called on a fixed time interval and processes the movement based on the movement value.
    /// </summary>
    private void FixedUpdate()
    {
        ProcessMovementValue(movementValue);
    }

    /// <summary>
    /// Sets the player's walkForward, moveAxisUsed, and isKeyDown variables to true, indicating movement in the forward direction.
    /// </summary>
    private void MovementTrue()
    {
        playerController.walkForward = true;

        playerController.moveAxisUsed = true;

        playerController.isKeyDown = true;
    }

    /// <summary>
    /// Sets the player's walkForward, moveAxisUsed, and isKeyDown variables to false, indicating no movement in the forward direction.
    /// </summary>
    private void MovementFalse()
    {
        playerController.walkForward = false;

        playerController.moveAxisUsed = false;

        playerController.isKeyDown = false;
    }

    /// <summary>
    /// Processes the movement based on the provided input.
    /// </summary>
    /// <param name="input">The input for horizontal and forward movement.</param>
    private void ProcessMovementValue(Vector2 input)
    {
        var parts0Transform = playerController.playerParts[0].transform;
        var parts0Rigidbody = parts0Transform.GetComponent<Rigidbody>();

        var horizontalInput = input.x;
        var forwardInput = input.y;

        // Move in camera direction
        playerController.direction = parts0Transform.rotation * new Vector3(horizontalInput, 0.0f, forwardInput);
        playerController.direction.y = 0f;

        var velocity = parts0Rigidbody.velocity;
        velocity = Vector3.Lerp(velocity, (playerController.direction * playerController.moveSpeed) + new Vector3(0, velocity.y, 0), 0.8f);
        parts0Rigidbody.velocity = velocity;

        switch (currentState)
        {
            case PlayerState.Balanced:

                if ((horizontalInput != 0 || forwardInput != 0) && playerController.balanced)
                {
                    if (!playerController.walkForward && !playerController.moveAxisUsed)
                    {
                        MovementTrue();
                    }
                }
                else
                {
                    currentState = PlayerState.NotBalanced;
                    MovementFalse();
                }

                break;

            case PlayerState.NotBalanced:

                if (horizontalInput == 0 && forwardInput == 0)
                {
                    if (playerController.walkForward && playerController.moveAxisUsed)
                    {
                        MovementFalse();
                    }
                }
                else
                {
                    currentState = PlayerState.Balanced;

                    MovementTrue();
                }

                break;
        }
    }
}