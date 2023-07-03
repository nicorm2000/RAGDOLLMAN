using UnityEngine;

/// <summary>
/// Applies ejection force to an item upon entering a portal.
/// </summary>
public class PortalEjectionForceApplier
{
    [Header("Physics Configuration")]
    [SerializeField] private float ejectionForce = 5f;

    /// <summary>
    /// Applies ejection force to the specified item.
    /// </summary>
    /// <param name="item">The item to apply ejection force to.</param>
    public void ApplyEjectionForce(GameObject item)
    {
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();

        if (itemRigidbody != null)
        {
            Vector3 ejectiondirection = Vector3.forward; // Or calculate based on the portal orientation

            itemRigidbody.AddForce(ejectiondirection * ejectionForce, ForceMode.Impulse);
        }
    }
}