using UnityEngine;

/// <summary>
/// Controls the background music, when should it play ot stop palying via collision.
/// </summary>
public class BackgroundMusicController : MonoBehaviour
{
    [Header("Audio Manager Dependencies")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private int musicIndex = 0;

    [Header("Player Configuration")]
    [SerializeField] private LayerMask playerLayer;

    private bool isInsideTrigger = false;

    /// <summary>
    /// Called when another collider enters the trigger.
    /// </summary>
    /// <param name="other">The other collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isInsideTrigger = true;

            PlayBackgroundMusic();
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger.
    /// </summary>
    /// <param name="other">The other collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isInsideTrigger = false;

            StopBackgroundMusic();
        }
    }

    /// <summary>
    /// Plays the background music using the AudioManager.
    /// </summary>
    private void PlayBackgroundMusic()
    {
        if (audioManager != null)
        {
            audioManager.PlayBackgroundMusic(musicIndex);
        }
    }

    /// <summary>
    /// Stops the currently playing background music using the AudioManager.
    /// </summary>
    private void StopBackgroundMusic()
    {
        if (audioManager != null)
        {
            audioManager.StopBackgroundMusic();
        }
    }
}