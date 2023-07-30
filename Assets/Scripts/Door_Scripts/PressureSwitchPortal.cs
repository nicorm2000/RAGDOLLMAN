using UnityEngine;

/// <summary>
/// Represents a pressure switch portal in the game.
/// Activates or deactivates a specified game object and triggers an animation when a player stays within its collider.
/// </summary>
public class PressureSwitchPortal : MonoBehaviour
{
    [Header("Switch Configuration")]
    [SerializeField] private GameObject currentObject;
    [SerializeField] private PressureSwitchAnimator switchAnimator;
    [SerializeField] private LayerMask playerLayer;

    [Header("Audio Manager Dependencies")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private int indexSFX = 0;

    private bool soundEffectPlayed = false;

    /// <summary>
    /// Called when a player stays within the collider.
    /// Activates the specified game object and triggers the switch animation.
    /// </summary>
    /// <param name="other">The collider of the other object.</param>
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentObject.SetActive(true);

            switchAnimator.SetSwitchState(true);

            if (!soundEffectPlayed && audioManager != null)
            {
                audioManager.PlaySoundEffect(indexSFX);

                soundEffectPlayed = true;
            }
        }
    }

    /// <summary>
    /// Called when a player exits the collider.
    /// Deactivates the specified game object and triggers the switch animation.
    /// </summary>
    /// <param name="other">The collider of the other object.</param>
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentObject.SetActive(false);

            switchAnimator.SetSwitchState(false);

            soundEffectPlayed = false;
        }
    }
}