using UnityEngine;

public class TriggerPanelController : MonoBehaviour
{
    [Header("Panel Dependencies")]
    [SerializeField] GameObject panel1; // Reference to the panel object in your Canvas
    [SerializeField] GameObject panel2; // Reference to the panel object in your Canvas

    [Header("Layer Dependencies")]
    [SerializeField] LayerMask playerLayer; // Layer mask representing the player

    /// <summary>
    /// Called when a GameObject enters the trigger collider.
    /// If the entering GameObject is the player, activates the panel.
    /// </summary>
    /// <param name="other">The Collider representing the other GameObject.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            panel1.SetActive(true);
            panel2.SetActive(true);
        }
    }

    /// <summary>
    /// Called when a GameObject exits the trigger collider.
    /// If the exiting GameObject is the player, deactivates the panel.
    /// </summary>
    /// <param name="other">The Collider representing the other GameObject.</param>
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            panel1.SetActive(false);
            panel2.SetActive(false);
        }
    }
}
