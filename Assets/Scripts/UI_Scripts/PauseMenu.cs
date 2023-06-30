using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction pause;

    [Header("Pause Configuration")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    /// <summary>
    /// Enables the pause action input and attaches the Pause method as the callback for the pause action.
    /// </summary>
    private void OnEnable()
    {
        pause = playerControls.Player.Pause;
        pause.Enable();

        pause.performed += Pause;
    }

    /// <summary>
    /// Disables the pause action input.
    /// </summary>
    private void OnDisable()
    {
        pause.Disable();
    }

    /// <summary>
    /// Toggles the pause state of the game. If the game is paused, activates the menu; otherwise, deactivates the menu.
    /// </summary>
    /// <param name="context">The callback context for the input action.</param>
    private void Pause(InputAction.CallbackContext context)
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
    public void DeactivateMenu()
    {
        Time.timeScale = 1;

        pauseUI.SetActive(false);

        isPaused = false;
    }

    /// <summary>
    /// Activates the pause menu, sets the time scale to 0, and updates the isPaused flag to true.
    /// </summary>
    public void ActivateMenu()
    {
        Time.timeScale = 0;

        pauseUI.SetActive(true);
    }
}
