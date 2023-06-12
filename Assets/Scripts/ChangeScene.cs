using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] int sceneToChangeTo;
    [SerializeField] float waitTimeBetweenScenes = 1.5f;
    [SerializeField] Animator transitionAnim;
    [SerializeField] LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");

        yield return new WaitForSeconds(waitTimeBetweenScenes);

        SceneManager.LoadScene(sceneToChangeTo);
    }
}
