using UnityEngine;

/// <summary>
/// Controls the patrolling behavior of an object between specified points.
/// </summary>
public class Patrol : MonoBehaviour
{
    [Header("Patrolling Configurations")]
    [SerializeField] Transform[] points;
    [SerializeField] float speed = 5f;

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
        if (transform.position != points[currentPosition].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentPosition].position, speed * Time.deltaTime);
            RotateTowards(points[currentPosition].position);
        }
        else
        {
            currentPosition = (currentPosition + 1) % points.Length;
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