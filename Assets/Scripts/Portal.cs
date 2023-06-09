using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportDestination;
    public float ejectionForce = 5f;
    public float teleportCooldown = 2f;

    private bool canTeleport = true;
    private Collider teleportCollider;

    private void Start()
    {
        teleportCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleport && other.CompareTag("Item"))
        {
            TeleportItem(other.gameObject);
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        canTeleport = false;
        teleportCollider.enabled = false; // Disable the teleporter collider

        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;
        teleportCollider.enabled = true; // Re-enable the teleporter collider
    }

    private void TeleportItem(GameObject item)
    {
        // Teleport the item to the destination
        item.transform.position = teleportDestination.position;

        // Eject the item with a specified force
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            Vector3 ejectionDirection = (teleportDestination.position - transform.position).normalized;
            itemRigidbody.AddForce(ejectionDirection * ejectionForce, ForceMode.Impulse);
        }
    }
}
