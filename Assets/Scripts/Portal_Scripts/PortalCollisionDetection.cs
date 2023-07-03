using UnityEngine;

/// <summary>
/// Detects collisions with a portal and performs teleportation of items or players.
/// </summary>
public class PortalCollisionDetection : MonoBehaviour
{
    [Header("Portal Configuration")]
    [SerializeField] private Transform teleportDestination;

    [Header("Item Configuration")]
    [SerializeField] private LayerMask itemLayer;

    [Header("Player Configuration")]
    [SerializeField] private LayerMask playerLayer;

    /// <summary>
    /// Called when another collider enters the portal collider.
    /// Teleports the item or all child objects of the player to the destination.
    /// </summary>
    /// <param name="other">The collider that entered the portal collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            ItemTeleporter.Instance.TeleportItem(other.gameObject, teleportDestination);
        }

        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerTeleporter.Instance.TeleportChildren(other.gameObject, teleportDestination);
        }
    }
}