using UnityEngine;

public class PortalEjectionForceApplier
{
    /// <summary>
    /// Applies ejection force to the specified item.
    /// </summary>
    /// <param name="item">The item to apply ejection force to.</param>
    public void ApplyEjectionForce(GameObject item)
    {
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            float ejectionForce = 5f; // Or use a configurable value
            Vector3 ejectionDirection = Vector3.forward; // Or calculate based on the portal orientation
            itemRigidbody.AddForce(ejectionDirection * ejectionForce, ForceMode.Impulse);
        }
    }
}