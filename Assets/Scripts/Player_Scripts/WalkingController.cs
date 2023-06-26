using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingController : MonoBehaviour
{
    public struct Step
    {
        bool shouldAlertLeg;

        Vector3 legPosition;
    }

    public struct WalkConfig
    {
        Step right;

        Step left;
    }

    [SerializeField] PlayerController playerController;

    /// <summary>
    /// Handles the player's walking.
    /// </summary>
    private void Walking()
    {

        WalkConfig prevConfig;
        WalkConfig newConfig;
        if (!playerController.inAir)
        {
            if (playerController.walkForward)
            {
                //Checks if the right leg is behind
                if (playerController.playerParts[11].transform.position.z < playerController.playerParts[12].transform.position.z && !playerController.stepLeft && !playerController.alertLegRight)
                {
                    
                    ShouldMoveLegRight();
                }

                //Checks if the left leg is behind
                if (playerController.playerParts[11].transform.position.z > playerController.playerParts[12].transform.position.z && !playerController.stepRight && !playerController.alertLegLeft)
                {
                    ShouldMoveLegLeft();
                }
            }

            if (playerController.walkBackward)
            {
                //Checks if the right leg is ahead
                if (playerController.playerParts[11].transform.position.z > playerController.playerParts[12].transform.position.z && !playerController.stepLeft && !playerController.alertLegRight)
                {
                    ShouldMoveLegRight();
                }

                //Checks if the left leg is ahead
                if (playerController.playerParts[11].transform.position.z < playerController.playerParts[12].transform.position.z && !playerController.stepRight && !playerController.alertLegLeft)
                {
                    ShouldMoveLegLeft();
                }
            }

            //prevConfig = newConfig;

            {
                    ////step right
                    //if (stepRight)
                    //{
                    ////Use fixedDeltaTime because of physics operations
                    //    stepTimerRight += Time.fixedDeltaTime;
                    //
                    //    //Right foot force down
                    //    playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //
                    //    //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                    //    //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                    //    //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                    //    if (walkForward)
                    //    {
                    //        playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * stepHeight, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * stepHeight * 2, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * stepHeight / 2, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //    }
                    //
                    //    //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                    //    //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                    //    //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                    //    if (walkBackward)
                    //    {
                    //        playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * stepHeight, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * stepHeight * 2, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * stepHeight / 2, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //    }
                    //
                    //    //Checks if the step_R_timer is greater than stepDuration, and if so, it sets step_R_timer back to zero,
                    //    //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                    //    if (stepTimerRight > stepDuration)
                    //    {
                    //        stepTimerRight = 0;
                    //
                    //        stepRight = false;
                    //
                    //        if (walkForward || walkBackward)
                    //        {
                    //            stepLeft = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //Reset to idle
                    //    //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[7] and playerParts[8] to quaternion values,
                    //    //that gradually interpolate back to their default values, scaled by specified time deltas. 
                    //    playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
                    //    playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);
                    //
                    //    //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                    //    //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                    //    playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //    playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //}
                    //
                    //
                    ////step left
                    //if (stepLeft)
                    //{
                    //    //Use fixedDeltaTime because of physics operations
                    //    stepTimerLeft += Time.fixedDeltaTime;
                    //
                    //    //Left foot force down
                    //    playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //
                    //    //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                    //    //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                    //    //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                    //    if (walkForward)
                    //    {
                    //        playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * stepHeight, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * stepHeight * 2, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * stepHeight / 2, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //    }
                    //
                    //    //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                    //    //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                    //    //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                    //    if (walkBackward)
                    //    {
                    //        playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * stepHeight, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * stepHeight * 2, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //        playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * stepHeight / 2, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    //    }
                    //
                    //    //Checks if the step_R_timer is greater than stepDuration, and if so, it sets step_R_timer back to zero,
                    //    //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                    //    if (stepTimerLeft > stepDuration)
                    //    {
                    //        stepTimerLeft = 0;
                    //
                    //        stepLeft = false;
                    //
                    //        if (walkForward || walkBackward)
                    //        {
                    //            stepRight = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //Reset to idle
                    //    //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[9] and playerParts[10] to quaternion values,
                    //    //that gradually interpolate back to their default values, scaled by specified time deltas. 
                    //    playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
                    //    playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);
                    //
                    //    //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                    //    //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                    //    playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //    playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                    //}

            }
        }
    }

    private void ShouldMoveLegLeft()
    {
        playerController.stepLeft = true;
        playerController.alertLegLeft = true;
        playerController.alertLegRight = true;
    }

    private void ShouldMoveLegRight()
    {
        playerController.stepRight = true;
        playerController.alertLegRight = true;
        playerController.alertLegLeft = true;
    }
}
