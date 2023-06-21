using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [Header("Player Dependencies")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerRoot;
    [SerializeField] Transform playerSpawn;

    [Header("Camera Settings")]
    [SerializeField] bool instantCameraUpdate = false;

    private Camera cam;
    private Rigidbody[] playerPhysics;
    private bool checkedTrigger;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkedTrigger)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(player.GetComponent<PlayerController>().thisPlayerLayer))
            {
                checkedTrigger = true;

                if (player != null)
                {
                    playerPhysics = player.GetComponentsInChildren<Rigidbody>();

                    //Deactivate physics
                    foreach (Rigidbody physics in playerPhysics)
                    {
                        physics.isKinematic = true;
                    }

                    //Record camera current offset
                    var cameraOffset = new Vector3(cam.transform.position.x - playerRoot.transform.position.x, cam.transform.position.y - playerRoot.transform.position.y, cam.transform.position.z - playerRoot.transform.position.z);

                    //Set player to new position
                    playerRoot.transform.localPosition = Vector3.zero;

                    player.transform.position = playerSpawn.position;

                    //Re-activate physics
                    foreach (Rigidbody physics in playerPhysics)
                    {
                        physics.isKinematic = false;
                    }

                    //Apply camera offset to new position
                    if (instantCameraUpdate)
                    {
                        cam.transform.position = playerRoot.transform.position + cameraOffset;
                    }
                }

                checkedTrigger = false;
            }
        }
    }
}