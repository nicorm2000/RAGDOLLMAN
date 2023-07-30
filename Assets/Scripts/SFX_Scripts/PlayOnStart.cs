using UnityEngine;

/// <summary>
/// Plays a sound effect on Start.
/// </summary>
public class PlayOnStart : MonoBehaviour
{
    [Header("Audio Manager Dependencies")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private int indexSFX = 0;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Start()
    {
        if (audioManager != null)
        {
            audioManager.PlaySoundEffect(indexSFX);
        }
    }
}