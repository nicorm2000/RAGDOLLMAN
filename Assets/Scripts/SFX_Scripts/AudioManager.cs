using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the audio for the game, both SFX and music.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    public List<AudioClip> soundEffects = new List<AudioClip>();

    [Header("Background Music")]
    public List<AudioClip> backgroundMusic = new List<AudioClip>();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private AudioSource backgroundMusicAudioSource;

    /// <summary>
    /// Plays a sound effect from the provided index.
    /// </summary>
    /// <param name="index">The index of the sound effect to play.</param>
    public void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Count)
        {
            soundEffectAudioSource.PlayOneShot(soundEffects[index]);
        }
    }

    /// <summary>
    /// Plays background music from the provided index.
    /// </summary>
    /// <param name="index">The index of the background music to play.</param>
    public void PlayBackgroundMusic(int index)
    {
        if (index >= 0 && index < backgroundMusic.Count)
        {
            backgroundMusicAudioSource.clip = backgroundMusic[index];
            backgroundMusicAudioSource.loop = true;
            backgroundMusicAudioSource.Play();
        }
    }

    /// <summary>
    /// Stops the currently playing background music.
    /// </summary>
    public void StopBackgroundMusic()
    {
        backgroundMusicAudioSource.Stop();
    }
}