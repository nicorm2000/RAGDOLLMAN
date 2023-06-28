﻿using UnityEngine;

public class Platform : MonoBehaviour
{
    //Physics platform
    public ConfigurableJoint platformJoint;
    //APR Controller script
    public PlayerController controller;


    private bool forward, pressed;


    void OnCollisionStay(Collision col)
    {
        ////Check if hand has joint, if so then check if it is connected to this object.
        //if(controller.RightHand.GetComponent<FixedJoint>() != null)
        //{
        //    if(controller.RightHand.GetComponent<FixedJoint>().connectedBody == gameObject.GetComponent<Rigidbody>())
        //    {
        //        if(forward && !pressed)
        //        {
        //            forward = false;
        //            pressed = true;
        //        }

        //        else if(!forward && !pressed)
        //        {
        //            forward = true;
        //            pressed = true;
        //        }
        //    }
        //}

        //if(controller.LeftHand.GetComponent<FixedJoint>() != null)
        //{
        //    if(controller.LeftHand.GetComponent<FixedJoint>().connectedBody == gameObject.GetComponent<Rigidbody>())
        //    {
        //        if(forward && !pressed)
        //        {
        //            forward = false;
        //            pressed = true;
        //        }

        //        else if(!forward && !pressed)
        //        {
        //            forward = true;
        //            pressed = true;
        //        }
        //    }
        //}
    }

    //Check if hand has joint, if so check if it is not this object
    void OnCollisionExit(Collision col)
    {
        if (pressed)
        {
            pressed = false;
        }
    }

    //Set joint target position
    void Update()
    {
        if (forward)
        {
            platformJoint.targetPosition = new Vector3(0, 0, 50f);
        }

        else if (!forward)
        {
            platformJoint.targetPosition = new Vector3(0, 0, 0);
        }
    }
}