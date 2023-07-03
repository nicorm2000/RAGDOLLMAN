using UnityEngine;

/// <summary>
/// Sets the default values for the scene.
/// </summary>
public class SceneSetter : MonoBehaviour
{
    [SerializeField] private float gameIsPlaying = 1f;
    /// <summary>
    /// Set scenes defaulta values if needed.
    /// </summary>
    private void Awake()
    {
        Time.timeScale = gameIsPlaying;
    }
}