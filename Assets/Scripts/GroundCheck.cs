using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    bool CheckIdleState(bool inAir, bool isJumping, bool reachRightAxisUsed, bool reachLeftAxisUsed)
    {
        if (!inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void GroundChecker(GameObject playerPart, float balanceHeight, bool inAir, bool isJumping, bool reachRightAxisUsed, bool reachLeftAxisUsed, bool balanced, bool autoGetUpWhenPossible)
    {
        Ray ray = new Ray(playerPart.transform.position, -playerPart.transform.up);

        RaycastHit hit;

        //Balance when ground is detected
        //1 << LayerMask.NameToLayer("Ground") -> Creates a bit mask with a 1 in the binary position of the layer "Ground".
        //Bit mask can be used to check if a layer is on or off in the Physics.Raycast function.
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && CheckIdleState(inAir, isJumping, reachRightAxisUsed, reachLeftAxisUsed))
        {
            if (!balanced && playerPart.GetComponent<Rigidbody>().velocity.magnitude < 1f)
            {
                if (autoGetUpWhenPossible)
                {
                    balanced = true;
                }
            }
        }
        //Fall over when ground is not detected
        else if (!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (balanced)
            {
                balanced = false;
            }
        }
    }
}