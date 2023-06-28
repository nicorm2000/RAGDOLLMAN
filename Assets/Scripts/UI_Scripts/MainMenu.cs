using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private int sceneToChangeTo;
    [SerializeField] private int creditsScene;
    [SerializeField] private int menuScene;

    /// <summary>
    /// Loads the scene specified by sceneToChangeTo when a button is clicked.
    /// </summary>
    public void ClickToStart()
    {
        //SceneTransitionController.Instance.StartTransition(sceneToChangeTo);
        SceneManager.LoadScene(sceneToChangeTo);
    }

    /// <summary>
    /// Loads the menu scene when a button is clicked.
    /// </summary>
    public void ClickToMenu()
    {
        //SceneTransitionController.Instance.StartTransition(sceneToChangeTo);
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Loads the credits scene when a button is clicked.
    /// </summary>
    public void ClickToCredits()
    {
        //SceneTransitionController.Instance.StartTransition(sceneToChangeTo);
        SceneManager.LoadScene(creditsScene);
    }

    /// <summary>
    /// Quits the application when a button is clicked.
    /// </summary>
    public void ClickToQuit() 
    { 
        Application.Quit();
    }
}