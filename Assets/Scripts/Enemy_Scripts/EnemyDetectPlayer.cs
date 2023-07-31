using UnityEngine;

/// <summary>
/// All the data detection information needed for the enemies.
/// </summary>
public class EnemyDetectPlayer : MonoBehaviour
{
    [Header("Player Dependencies")]
    public GameObject player;
    public GameObject playerRoot;
    public Transform playerSpawn;

    [Header("GodMode Dependencies")]
    public GodMode godModeScript;
    public bool forceRespawn = false;

    [Header("Audio Manager Dependencies")]
    public AudioManager audioManager;
    public int indexSFX = 2;
}