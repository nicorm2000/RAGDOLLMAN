using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the lerping movement between two game objects.
/// </summary>
public class LerpingController : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Lerping Settings")]
    [SerializeField] private float lerpDuration = 1f;
    [SerializeField] private float waitTime = 3f;

    private IEnumerator currentLerpCoroutine;

    /// <summary>
    /// Starts the lerping process.
    /// </summary>
    private void OnEnable()
    {
        StartLerping();
    }

    /// <summary>
    /// Stops the lerping process.
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(currentLerpCoroutine);
    }

    /// <summary>
    /// Starts the lerping process.
    /// </summary>
    public void StartLerping()
    {
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }

        currentLerpCoroutine = LerpingCoroutine();

        StartCoroutine(currentLerpCoroutine);
    }

    /// <summary>
    /// Main lerping loop that goes back and forth between the two game objects indefinitely.
    /// </summary>
    private IEnumerator LerpingCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(LerpPosition(pointA.position, pointB.position, lerpDuration));

            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(LerpPosition(pointB.position, pointA.position, lerpDuration));

            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Lerps the position of the current object from start to end over a given duration.
    /// </summary>
    /// <param name="start">The start position of the lerp.</param>
    /// <param name="end">The end position of the lerp.</param>
    /// <param name="time">The duration of the lerp.</param>
    public IEnumerator LerpPosition(Vector3 start, Vector3 end, float time)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;

            transform.position = Vector3.Lerp(start, end, t);

            yield return null;
        }

        transform.position = end;   
    }
}