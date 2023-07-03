using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the scene transitions with animations and delays.
/// </summary>
public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController instance;
    public static SceneTransitionController Instance => instance;

    [Header("Transition Configuration")]
    [SerializeField] private float waitTimeBetweenScenes = 1.2f;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private string transitionTriggerName = "End";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Initiates the transition to the specified scene.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to transition to.</param>
    public void StartTransition(int sceneIndex)
    {
        StartCoroutine(Transition(sceneIndex));
    }

    /// <summary>
    /// Coroutine that performs the transition animation and loads the specified scene after a delay.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to transition to.</param>
    /// <returns>The IEnumerator object.</returns>
    private IEnumerator Transition(int sceneIndex)
    {
        transitionAnim.SetTrigger(transitionTriggerName);

        yield return new WaitForSeconds(waitTimeBetweenScenes);

        SceneManager.LoadScene(sceneIndex);
    }
}