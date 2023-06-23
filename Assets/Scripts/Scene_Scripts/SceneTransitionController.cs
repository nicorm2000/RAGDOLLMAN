using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController instance;
    public static SceneTransitionController Instance => instance;

    [Header("Transition Configuration")]
    [SerializeField] float waitTimeBetweenScenes = 1.2f;
    [SerializeField] Animator transitionAnim;

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
        transitionAnim.SetTrigger("End");

        yield return new WaitForSeconds(waitTimeBetweenScenes);

        SceneManager.LoadScene(sceneIndex);
    }
}