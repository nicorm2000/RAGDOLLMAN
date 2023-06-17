using UnityEngine;

public class HandContact : MonoBehaviour
{
    [SerializeField] APRController APR_Player;

    //Is left or right hand
    [SerializeField] bool Left;

    //Have joint/grabbed
    [SerializeField] bool hasJoint;

    void Update()
    {
        if (APR_Player.useControls)
        {
            //Left Hand
            //On input release destroy joint
            if (Left)
            {
                if (hasJoint && Input.GetAxisRaw(APR_Player.reachLeft) == 0)
                {
                    gameObject.GetComponent<FixedJoint>().breakForce = 0;

                    hasJoint = false;
                }

                if (hasJoint && gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }

            //Right Hand
            //On input release destroy joint
            if (!Left)
            {
                if (hasJoint && Input.GetAxisRaw(APR_Player.reachRight) == 0)
                {
                    gameObject.GetComponent<FixedJoint>().breakForce = 0;

                    hasJoint = false;
                }

                if (hasJoint && gameObject.GetComponent<FixedJoint>() == null)
                {
                    hasJoint = false;
                }
            }
        }
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        if (APR_Player.useControls)
        {
            //Left Hand
            if (Left)
            {
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    if (Input.GetAxisRaw(APR_Player.reachLeft) != 0 && !hasJoint && !APR_Player.punchingLeft)
                    {
                        hasJoint = true;

                        gameObject.AddComponent<FixedJoint>();
                        gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                    }
                }
            }

            //Right Hand
            if (!Left)
            {
                if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
                {
                    if (Input.GetAxisRaw(APR_Player.reachRight) != 0 && !hasJoint && !APR_Player.punchingRight)
                    {
                        hasJoint = true;

                        gameObject.AddComponent<FixedJoint>();
                        gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                        gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                    }
                }
            }
        }
    }
}