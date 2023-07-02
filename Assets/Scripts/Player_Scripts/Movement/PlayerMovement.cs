using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    private Vector2 movementValue = Vector2.zero;

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

        // The player is balanced if this is true
        if ((horizontalInput != 0 || forwardInput != 0) && playerController.balanced)
        {
            if (!playerController.walkForward && !playerController.moveAxisUsed)
            {
                MovementTrue();
            }
        }
        else if (horizontalInput == 0 && forwardInput == 0)
        {
            if (playerController.walkForward && playerController.moveAxisUsed)
            {
                MovementFalse();
            }
        }
    }

    public void SetMovement(Vector2 normalizedMovement) => movementValue = normalizedMovement;
}