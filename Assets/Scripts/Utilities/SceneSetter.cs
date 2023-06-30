using UnityEngine;

public class SceneSetter : MonoBehaviour
{
    /// <summary>
    /// Set scenes defaulta values if needed.
    /// </summary>
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
}