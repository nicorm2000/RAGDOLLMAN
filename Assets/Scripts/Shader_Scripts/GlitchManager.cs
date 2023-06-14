using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    [Header("Camera Selector")]
    [SerializeField] GlitchEffect glitchCamera;

    [Header("Player Configuration")]
    [SerializeField] private Transform playerHips;

    [Header("Radius Configuration")]
    [SerializeField] private Transform centerOfObject;
    [SerializeField] private float maxDistance;

    [Header("Glitch Configuration")]
    [SerializeField] private float maxGlitchIntensity = 0.5f;
    [SerializeField] private float maxGlitchFlipIntensity= 0.5f;
    [SerializeField] private float maxGlitchColorIntensity = 0.5f;

    private void Start()
    {
        if (glitchCamera == null) 
        { 
            gameObject.AddComponent<GlitchEffect>();
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(centerOfObject.position, playerHips.position);
        
        if (distance < maxDistance)
        {
            UpdateGlitchEffects(distance);
        }
        else
        {
            ResetGlitchEffects();
        }
    }

    private void ResetGlitchEffects()
    {
        glitchCamera.intensity = 0;
        glitchCamera.flipIntensity = 0;
        glitchCamera.colorIntensity = 0;
    }

    private void UpdateGlitchEffects(float distance)
    {
        float insidePercentage = distance / maxDistance;

        glitchCamera.intensity = Mathf.Lerp(maxGlitchIntensity, 0, insidePercentage);
        glitchCamera.flipIntensity = Mathf.Lerp(maxGlitchFlipIntensity, 0, insidePercentage);
        glitchCamera.colorIntensity = Mathf.Lerp(maxGlitchColorIntensity, 0, insidePercentage);
    }
}