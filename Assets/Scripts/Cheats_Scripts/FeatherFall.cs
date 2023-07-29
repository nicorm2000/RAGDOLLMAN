using UnityEngine;

/// <summary>
/// Controls the feather fall effect for the player character.
/// </summary>
public class FeatherFall : MonoBehaviour
{
    [Header("Player Controller Configuration")]
    [SerializeField] PlayerController playerController;

    [SerializeField] float featherFallMultiplier = 0.25f; // Adjust this value as needed

    private Rigidbody[] playerRigidbodies;
    private bool isFeatherFalling = false;
    private Vector3 originalGravity; // Store the original gravity

    /// <summary>
    /// Initializes the component by getting the player's rigidbodies and storing the original gravity.
    /// </summary>
    private void Start()
    {
        playerRigidbodies = GetComponentsInChildren<Rigidbody>();

        originalGravity = Physics.gravity;
    }

    /// <summary>
    /// Applies the feather fall effect by adjusting the velocity of the player's rigidbodies.
    /// </summary>
    private void FixedUpdate()
    {
        if (isFeatherFalling)
        {
            foreach (var rb in playerRigidbodies)
            {
                rb.velocity *= featherFallMultiplier;
            }
        }
    }

    /// <summary>
    /// Toggles the feather fall effect on or off.
    /// </summary>
    public void ToggleFeatherFall()
    {
        isFeatherFalling = !isFeatherFalling;

        if (isFeatherFalling)
        {
            Physics.gravity *= featherFallMultiplier;
        }
        else
        {
            Physics.gravity = originalGravity;
        }
    }
}