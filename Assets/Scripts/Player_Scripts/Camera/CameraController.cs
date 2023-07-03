using UnityEngine;

/// <summary>
/// Controls the camera movement and rotation to follow a target object.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Player To Follow")]
    //Player root
    public Transform APRRoot;

    [Header("Follow Properties")]
    //Follow values
    public float distanceY = 10.0f;
    public float distanceZ = 10.0f;
    public float smoothness = 0.15f;

    [Header("Rotation Properties")]
    //Rotate with input, min & max camera angle
    public float rotateSpeed = 5.0f;
    public float minAngle = -45.0f;
    public float maxAngle = -10.0f;

    [HideInInspector] public Camera cam;
    [HideInInspector] public float currentX = 0.0f;
    [HideInInspector] public float currentY = 0.0f;
    [HideInInspector] public Quaternion rotation;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Vector3 offset;

    /// <summary>
    /// Locks cursor, makes it invisible, sets camera, and its offset.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main;

        offset = cam.transform.position;
    }

    /// <summary>
    /// If rotateCamera is true, rotates camera to look at APRRoot and moves it based on current X, Y, and distance. 
    /// If rotateCamera is false, faces the camera towards APRRoot and moves the camera to a fixed offset position.
    /// </summary>
    void FixedUpdate()
    {
        currentY = Mathf.Clamp(currentY, minAngle, maxAngle);

        dir = new Vector3(0, -distanceY, -distanceZ);

        rotation = Quaternion.Euler(-currentY, currentX, 0);

        cam.transform.position = Vector3.Lerp(cam.transform.position, APRRoot.position + rotation * dir, smoothness);
        cam.transform.LookAt(APRRoot.position);
    }
}