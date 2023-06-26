using UnityEngine;

public class CheckerGround : MonoBehaviour
{
    /// <summary>
    /// Determines if the character is in an idle state.
    /// </summary>
    /// <param name="inAir">Indicates if the character is in the air.</param>
    /// <param name="isJumping">Indicates if the character is currently jumping.</param>
    /// <param name="reachRightAxisUsed">Indicates if the character's right axis is being used.</param>
    /// <param name="reachLeftAxisUsed">Indicates if the character's left axis is being used.</param>
    /// <returns>Returns true if the character is idle; otherwise, returns false.</returns>
    bool IsIdle(bool inAir, 
        bool isJumping, 
        bool reachRightAxisUsed, 
        bool reachLeftAxisUsed)
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

    /// <summary>
    /// Checks if the character is grounded and adjusts balance accordingly.
    /// </summary>
    /// <param name="playerPart">The GameObject representing a part of the player.</param>
    /// <param name="balanceHeight">The height at which balance is maintained.</param>
    /// <param name="inAir">Indicates if the character is in the air.</param>
    /// <param name="isJumping">Indicates if the character is currently jumping.</param>
    /// <param name="reachRightAxisUsed">Indicates if the character's right axis is being used.</param>
    /// <param name="reachLeftAxisUsed">Indicates if the character's left axis is being used.</param>
    /// <param name="balanced">A reference to the balance state of the character.</param>
    /// <param name="autoGetUpWhenPossible">Indicates if the character should automatically get up when possible.</param>
    public void GroundChecker(Joint playerPart,
        float balanceHeight,
        bool inAir,
        bool isJumping,
        bool reachRightAxisUsed,
        bool reachLeftAxisUsed,
        ref bool balanced,
        bool autoGetUpWhenPossible)
    {
        Ray ray = new Ray(playerPart.transform.position, -playerPart.transform.up);

        RaycastHit hit;

        //Balance when ground is detected
        //1 << LayerMask.NameToLayer("Ground") -> Creates a bit mask with a 1 in the binary position of the layer "Ground".
        //Bit mask can be used to check if a layer is on or off in the Physics.Raycast function.
        if (Physics.Raycast(ray,
            out hit,
            balanceHeight,
            1 << LayerMask.NameToLayer("Ground")) && IsIdle(inAir, 
            isJumping, 
            reachRightAxisUsed, 
            reachLeftAxisUsed))
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
        else if (!Physics.Raycast(ray, 
            out hit, 
            balanceHeight, 
            1 << LayerMask.NameToLayer("Ground")))
        {
            if (balanced)
            {
                balanced = false;
            }
        }
    }
}