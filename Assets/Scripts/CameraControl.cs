using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float stomachOffset;
    [SerializeField] float minCamera = -35f;
    [SerializeField] float maxCamera = 60f;
    [SerializeField] Transform root;
    [SerializeField] ConfigurableJoint hipJoint;
    [SerializeField] ConfigurableJoint stomachJoint;

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;

        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        mouseY = Mathf.Clamp(mouseY, minCamera, maxCamera);

        Quaternion rootRotation = Quaternion.Euler(mouseY, mouseX, 0);

        root.rotation = rootRotation;

        hipJoint.targetRotation = Quaternion.Euler(0, -mouseX, 0);
        
        stomachJoint.targetRotation = Quaternion.Euler(-mouseY + stomachOffset, 0, 0);
    }
}
