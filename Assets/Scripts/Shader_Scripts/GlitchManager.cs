using UnityEditor;
using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    [Header("Camera Selector")]
    [SerializeField] private GlitchEffect glitchCamera;

    [Header("Player Configuration")]
    [SerializeField] private Transform playerHips;

    [Header("Radius Configuration")]
    [SerializeField] private Transform centerOfObject;
    [SerializeField] private float maxDistance;

    [Header("Glitch Configuration")]
    [SerializeField] private float maxGlitchIntensity = 0.5f;
    [SerializeField] private float maxGlitchFlipIntensity = 0.5f;
    [SerializeField] private float maxGlitchColorIntensity = 0.5f;

    private void Start()
    {
        // Ensure glitchCamera is assigned, otherwise add GlitchEffect component to the game object
        if (glitchCamera == null)
        {
            glitchCamera = gameObject.AddComponent<GlitchEffect>();
        }
    }

    private void Update()
    {
        // Calculate the distance between centerOfObject and playerHips
        float distance = Vector3.Distance(centerOfObject.position, playerHips.position);

        if (distance < maxDistance)
        {
            ApplyGlitchEffects(distance);
        }
        else
        {
            ResetGlitchEffects();
        }
    }

    /// <summary>
    /// Resets the glitch effects to their default values.
    /// </summary>
    private void ResetGlitchEffects()
    {
        glitchCamera.intensity = 0;
        glitchCamera.flipIntensity = 0;
        glitchCamera.colorIntensity = 0;
    }

    /// <summary>
    /// Updates the glitch effects based on the distance between the player and the center of the object.
    /// </summary>
    /// <param name="distance">The distance between the player and the center of the object.</param>
    private void ApplyGlitchEffects(float distance)
    {
        // Calculate the percentage of distance inside the maxDistance range
        float insidePercentage = distance / maxDistance;

        // Apply glitch effects using a linear interpolation based on the insidePercentage
        glitchCamera.intensity = Mathf.Lerp(maxGlitchIntensity, 0, insidePercentage);
        glitchCamera.flipIntensity = Mathf.Lerp(maxGlitchFlipIntensity, 0, insidePercentage);
        glitchCamera.colorIntensity = Mathf.Lerp(maxGlitchColorIntensity, 0, insidePercentage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerOfObject.transform.position, maxDistance);
    }
}
