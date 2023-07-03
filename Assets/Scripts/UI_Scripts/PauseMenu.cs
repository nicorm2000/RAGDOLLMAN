using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the functionality of the pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("Pause Configuration")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;
    [SerializeField] private int NotPausedValue = 1;
    [SerializeField] private int PausedValue = 0;

    /// <summary>
    /// Toggles the pause state of the game. If the game is paused, activates the menu; otherwise, deactivates the menu.
    /// </summary>
    /// <param name="context">The callback context for the input action.</param>
    public void OnPause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused) 
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    /// <summary>
    /// Deactivates the pause menu, sets the time scale to 1, and updates the isPaused flag to false.
    /// </summary>
    private void DeactivateMenu()
    {
        Time.timeScale = NotPausedValue;

        pauseUI.SetActive(false);

        isPaused = false;
    }

    /// <summary>
    /// Activates the pause menu, sets the time scale to 0, and updates the isPaused flag to true.
    /// </summary>
    private void ActivateMenu()
    {
        Time.timeScale = PausedValue;

        pauseUI.SetActive(true);
    }
}