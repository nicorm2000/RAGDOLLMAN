using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the patrolling behavior of an object between specified points.
/// </summary>
public class EnemyPatrol : MonoBehaviour, IEnemyFactory
{
    [Header("Patrolling Configurations")]
    public List <Transform> points;
    public float speed = 5f;
    public RespawnPlayer detectionZone;

    private int currentPosition;

    /// <summary>
    /// Initializes the starting position of the object.
    /// </summary>
    private void Start()
    {
        currentPosition = 0;
    }

    /// <summary>
    /// Moves the object towards the next point in the patrol route and rotates it to face the point.
    /// </summary>
    private void Update()
    {
        Patrol();
    }

    /// <summary>
    /// Initializes the enemy detection zone with the provided data.
    /// </summary>
    /// <param name="detectData">The data used to initialize the detection zone.</param>
    public void Init(EnemyDetectPlayer detectData)
    {
        detectionZone.player = detectData.player;
        detectionZone.playerRoot = detectData.playerRoot;
        detectionZone.playerSpawn = detectData.playerSpawn;
        detectionZone.godModeScript = detectData.godModeScript;
        detectionZone.forceRespawn = detectData.forceRespawn;
        detectionZone.audioManager = detectData.audioManager;
        detectionZone.indexSFX = detectData.indexSFX;
    }

    /// <summary>
    /// Moves the enemy towards the patrol points and rotates towards the next point.
    /// </summary>
    public void Patrol()
    {
        if (transform.position != points[currentPosition].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentPosition].position, speed * Time.deltaTime);
            RotateTowards(points[currentPosition].position);
        }
        else
        {
            currentPosition = (currentPosition + 1) % points.Count;
        }
    }

    /// <summary>
    /// Rotates the object to face the given target position.
    /// </summary>
    /// <param name="target">The target position to face.</param>
    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);
    }
}