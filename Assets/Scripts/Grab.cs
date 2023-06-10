using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int breakForce = 9001;
    [SerializeField] int isLeftOrRight;
    [SerializeField] LayerMask itemLayer;

    private Rigidbody rb;
    private GameObject grabbedObject;
    private FixedJoint fj;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(isLeftOrRight))
        {
            if (isLeftOrRight == 0)
            {
                animator.SetBool("isLeftHandUp", true);
            }
            else if (isLeftOrRight == 1)
            {
                animator.SetBool("isRightHandUp", true);
            }

            if (grabbedObject != null)
            {
                fj = grabbedObject.AddComponent<FixedJoint>();

                fj.connectedBody = rb;

                fj.breakForce = breakForce;
            }
        }
        else if (Input.GetMouseButtonUp(isLeftOrRight))
        {
            if (isLeftOrRight == 0)
            {
                animator.SetBool("isLeftHandUp", false);
            }
            else if (isLeftOrRight == 1)
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
        if (((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            grabbedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            grabbedObject = null;

        }
    }
}
