using UnityEngine;

/// <summary>
/// Respawns the player when they enter a trigger collider, resetting their position and physics.
/// </summary>
public class RespawnPlayer : MonoBehaviour
{
    [Header("Player Dependencies")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private Transform playerSpawn;

    [Header("Camera Settings")]
    [SerializeField] private bool instantCameraUpdate = false;

    [Header("GodMode Dependencies")]
    [SerializeField] private GodMode godModeScript;//Reference to the GodMode script
    [SerializeField] private bool forceRespawn = false;//Whether to force respawn regardless of "God mode"

    [Header("Audio Manager Dependencies")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private int indexSFX = 2;

    private Camera cam;
    private Rigidbody[] playerPhysics;
    private bool checkedTrigger;

    private void Awake()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Called when another collider enters the trigger collider.
    /// Respawns the player, resetting their position and physics.
    /// </summary>
    /// <param name="other">The collider that entered the trigger collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!checkedTrigger)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(player.GetComponent<PlayerController>().thisPlayerLayer))
            {
                if (audioManager != null)
                {
                    audioManager.PlaySoundEffect(indexSFX);
                }

                checkedTrigger = true;

                if (!godModeScript.godMode || forceRespawn)
                {

                    if (player != null)
                    {
                        playerPhysics = player.GetComponentsInChildren<Rigidbody>();

                        //Deactivate physics
                        foreach (Rigidbody physics in playerPhysics)
                        {
                            physics.isKinematic = true;
                        }

                        //Record camera current offset
                        var cameraOffset = new Vector3(cam.transform.position.x - playerRoot.transform.position.x,
                            cam.transform.position.y - playerRoot.transform.position.y,
                            cam.transform.position.z - playerRoot.transform.position.z);

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
                }

                checkedTrigger = false;
            }
        }
    }
}