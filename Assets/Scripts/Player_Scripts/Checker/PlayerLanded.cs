using UnityEngine;

/// <summary>
/// Checks if the player has landed and updates the relevant variables accordingly.
/// </summary>
public class PlayerLanded : MonoBehaviour
{
    /// <summary>
    /// Checks if the player has landed by checking the values of `inAir`, `isJumping`, and `jumping`. If all three are false and the player has just landed, sets `inAir` to false and `resetPose` to true.
    /// </summary>
    /// <param name="inAir">A boolean representing whether or not the player is in the air.</param>
    /// <param name="isJumping">A boolean representing whether or not the player is currently jumping.</param>
    /// <param name="jumping">A boolean representing whether or not the player has attempted to jump.</param>
    /// <param name="resetPose">A boolean representing whether or not to reset the player's pose.</param>
    public void PlayerHasLanded(ref bool inAir, 
        bool isJumping, 
        bool jumping, 
        ref bool resetPose)
    {
        bool playerLanded = inAir && !isJumping && !jumping;

        if (playerLanded)
        {
            inAir = false;
            resetPose = true;
        }
    }
}