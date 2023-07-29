using UnityEngine;
/// <summary>
/// Controls the flash ability for the player character.
/// </summary>
public class Flash : MonoBehaviour
{
    [Header("Player Controller Configuration")]
    [SerializeField] PlayerController playerController;
    [SerializeField] float flashMoveSpeedMultiplier = 3f;

    private bool toggleFlash = false;

    /// <summary>
    /// Toggles the flash ability on or off, adjusting the player's move speed accordingly.
    /// </summary>
    public void ToggleFlash()
    {
        toggleFlash = !toggleFlash;

        if (toggleFlash)
        {
            playerController.moveSpeed *= flashMoveSpeedMultiplier;
        }
        else
        {
            playerController.moveSpeed /= flashMoveSpeedMultiplier;
        }
    }
}