using UnityEngine;
using UnityEngine.InputSystem;

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

    //Private variables
    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private Quaternion rotation;
    private Vector3 dir;
    private Vector3 offset;

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

    /// <summary>
    /// Callback function for mouse camera movement input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMouseCameraMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentX += context.ReadValue<Vector2>().x * rotateSpeed * Time.deltaTime;
            currentY += context.ReadValue<Vector2>().y * rotateSpeed * Time.deltaTime;
        }
        else
        {
            currentX += 0;
            currentY += 0;
        }
    }

    /// <summary>
    /// Callback function for controller camera movement input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnControllerCameraMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentX += context.ReadValue<Vector2>().x * rotateSpeed * 10 * Time.deltaTime;
            currentY -= context.ReadValue<Vector2>().y * rotateSpeed * 10 * Time.deltaTime;
        }
        else
        {
            currentX += 0;
            currentY += 0;
        }
    }
}