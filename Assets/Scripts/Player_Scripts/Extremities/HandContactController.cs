using UnityEngine;

public class HandContactController : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Decide Hand")]
    //Is left or right hand
    [SerializeField] private bool Left;

    //Have joint/grabbed
    private bool hasJoint;

    void Update()
    {
        //Left Hand
        //On input release destroy joint
        if (Left)
        {
            if (hasJoint && Input.GetAxisRaw(playerController.reachLeft) == 0)
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
            if (hasJoint && Input.GetAxisRaw(playerController.reachRight) == 0)
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

    /// <summary>
    /// Checks if the playerController is using controls. If so, checks if the Left boolean is true or false, and if the collided object has a "CanBeGrabbed" tag and is on a different layer than the player. If these conditions are met and the player is not already holding an object, checks if the reachLeft or reachRight input axes are non-zero and the other hand is not punching. If all of these conditions are met, adds a FixedJoint component to the player's GameObject and sets it to the collided object's Rigidbody component. The Left boolean is used to determine which hand is used.
    /// </summary>
    /// <param name="col">The Collision object representing the collision that occurred.</param>
    void OnCollisionEnter(Collision col)
    {
        //Left Hand
        if (Left)
        {
            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(playerController.thisPlayerLayer) && !hasJoint)
            {
                if (Input.GetAxisRaw(playerController.reachLeft) != 0 && !hasJoint && !playerController.punchingLeft)
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
            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(playerController.thisPlayerLayer) && !hasJoint)
            {
                if (Input.GetAxisRaw(playerController.reachRight) != 0 && !hasJoint && !playerController.punchingRight)
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