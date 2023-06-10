using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform teleportDestination;

    [SerializeField] float ejectionForce = 5f;

    [SerializeField] LayerMask itemLayer;
    [SerializeField] LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            TeleportItem(other.gameObject);
        }

        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            for (int i = 0; i < other.transform.parent.childCount; i++)
            {
                TeleportItem(other.transform.parent.GetChild(i).gameObject);
            }
        }
    }

    private void TeleportItem(GameObject item)
    {
        // Teleport the item to the destination
        item.transform.position = teleportDestination.position;

        // Calculate the ejection direction as the forward direction of the world
        Vector3 ejectionDirection = Vector3.forward;

        // Apply the ejection force in the calculated direction
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();

        if (itemRigidbody != null)
        {
            itemRigidbody.AddForce(ejectionDirection * ejectionForce, ForceMode.Impulse);
        }
    }
}
