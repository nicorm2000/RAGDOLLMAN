using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int breakForce = 9001;
    [SerializeField] int isLeftToRight;
    [SerializeField] bool alreadyGrabbing = false;

    private Rigidbody rb;
    private GameObject grabbedObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(isLeftToRight))
        {
            if (isLeftToRight == 0)
            {
                animator.SetBool("isLeftHandUp", true);
            }
            else if (isLeftToRight == 1)
            {
                animator.SetBool("isRightHandUp", true);
            }

            FixedJoint fj = grabbedObject.AddComponent<FixedJoint>();

            fj.connectedBody = rb;

            fj.breakForce = breakForce;
        }
        else if (Input.GetMouseButtonUp(isLeftToRight))
        {
            if (isLeftToRight == 0)
            {
                animator.SetBool("isLeftHandUp", false);
            }
            else if (isLeftToRight == 1)
            {
                animator.SetBool("isRightHandUp", false);
            }

            if (grabbedObject != null)
            {
                Destroy(grabbedObject.GetComponent<FixedJoint>());
            }

            grabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            grabbedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        grabbedObject = null;
    }
}
