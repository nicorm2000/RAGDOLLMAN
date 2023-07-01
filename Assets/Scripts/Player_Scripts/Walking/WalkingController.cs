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

    [Header("Player Controller Dependencies")]
    [SerializeField] public PlayerController playerController;

    [Header("Walking Modifiers")]
    [SerializeField] private float stepHeightModifier = 2.0f;
    [SerializeField] private float xRotationModifier1 = 0.09f;
    [SerializeField] private float xRotationModifier2 = 0.09f;
    [SerializeField] private float xRotationModifier3 = 0.12f;
    [SerializeField] private float xRotationModifier4 = 0.00f;
    [SerializeField] private float xRotationModifier5 = 0.07f;
    [SerializeField] private float xRotationModifier6 = 0.02f;
    [SerializeField] private float legTarget1 = 8f;
    [SerializeField] private float legTarget2 = 17f;
    [SerializeField] private float legTarget3 = 7f;
    [SerializeField] private float legTarget4 = 18f;

    /// <summary>
    /// Handles the player's walking.
    /// </summary>
    public void Walking()
    {
        if (!playerController.inAir)
        {
            if (playerController.walkForward)
            {
                //Checks if the right leg is behind
                if (playerController.playerParts[11].transform.position.z
                    < playerController.playerParts[12].transform.position.z
                    && !playerController.stepLeft && !playerController.alertLegRight)
                {
                    ShouldMoveLegRight();
                }

                //Checks if the left leg is behind
                if (playerController.playerParts[11].transform.position.z
                    > playerController.playerParts[12].transform.position.z
                    && !playerController.stepRight && !playerController.alertLegLeft)
                {
                    ShouldMoveLegLeft();
                }
            }

            if (playerController.walkBackward)
            {
                //Checks if the right leg is ahead
                if (playerController.playerParts[11].transform.position.z
                    > playerController.playerParts[12].transform.position.z
                    && !playerController.stepLeft && !playerController.alertLegRight)
                {
                    ShouldMoveLegRight();
                }

                //Checks if the left leg is ahead
                if (playerController.playerParts[11].transform.position.z
                    < playerController.playerParts[12].transform.position.z && !playerController.stepRight
                    && !playerController.alertLegLeft)
                {
                    ShouldMoveLegLeft();
                }
            }

            //step right
            if (playerController.stepRight)
            {
                //Use fixedDeltaTime because of physics operations
                playerController.stepTimerRight += Time.fixedDeltaTime;

                //Right foot force down
                playerController.playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                if (playerController.walkForward)
                {
                    playerController.playerParts[7].targetRotation = new Quaternion(playerController.playerParts[7].targetRotation.x + 0.09f * playerController.stepHeight,
                        playerController.playerParts[7].targetRotation.y,
                        playerController.playerParts[7].targetRotation.z,
                        playerController.playerParts[7].targetRotation.w);
                    playerController.playerParts[8].targetRotation = new Quaternion(playerController.playerParts[8].targetRotation.x - 0.09f * playerController.stepHeight * 2,
                        playerController.playerParts[8].targetRotation.y,
                        playerController.playerParts[8].targetRotation.z,
                        playerController.playerParts[8].targetRotation.w);
                    playerController.playerParts[9].targetRotation = new Quaternion(playerController.playerParts[9].targetRotation.x - 0.12f * playerController.stepHeight / 2,
                        playerController.playerParts[9].targetRotation.y,
                        playerController.playerParts[9].targetRotation.z,
                        playerController.playerParts[9].targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                if (playerController.walkBackward)
                {
                    playerController.playerParts[7].targetRotation = new Quaternion(playerController.playerParts[7].targetRotation.x - 0.00f * playerController.stepHeight
                        , playerController.playerParts[7].targetRotation.y,
                        playerController.playerParts[7].targetRotation.z,
                        playerController.playerParts[7].targetRotation.w);
                    playerController.playerParts[8].targetRotation = new Quaternion(playerController.playerParts[8].targetRotation.x - 0.07f * playerController.stepHeight * 2,
                        playerController.playerParts[8].targetRotation.y,
                        playerController.playerParts[8].targetRotation.z,
                        playerController.playerParts[8].targetRotation.w);
                    playerController.playerParts[9].targetRotation = new Quaternion(playerController.playerParts[9].targetRotation.x + 0.02f * playerController.stepHeight / 2,
                        playerController.playerParts[9].targetRotation.y,
                        playerController.playerParts[9].targetRotation.z,
                        playerController.playerParts[9].targetRotation.w);
                }

                //Checks if the stepTimerRight is greater than stepDuration, and if so, it sets stepTimerRight back to zero,
                //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                if (playerController.stepTimerRight > playerController.stepDuration)
                {
                    ResetStepRight();
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[7] and playerParts[8] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                playerController.playerParts[7].targetRotation = Quaternion.Lerp(playerController.playerParts[7].targetRotation,
                    playerController.UpperRightLegTarget,
                    8f * Time.fixedDeltaTime);
                playerController.playerParts[8].targetRotation = Quaternion.Lerp(playerController.playerParts[8].targetRotation,
                    playerController.LowerRightLegTarget,
                    17f * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                playerController.playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);
                playerController.playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);
            }


            //step left
            if (playerController.stepLeft)
            {
                //Use fixedDeltaTime because of physics operations
                playerController.stepTimerLeft += Time.fixedDeltaTime;

                //Left foot force down
                playerController.playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                if (playerController.walkForward)
                {
                    playerController.playerParts[9].targetRotation = new Quaternion(playerController.playerParts[9].targetRotation.x + 0.09f * playerController.stepHeight,
                        playerController.playerParts[9].targetRotation.y,
                        playerController.playerParts[9].targetRotation.z,
                        playerController.playerParts[9].targetRotation.w);
                    playerController.playerParts[10].targetRotation = new Quaternion(playerController.playerParts[10].targetRotation.x - 0.09f * playerController.stepHeight * 2,
                        playerController.playerParts[10].targetRotation.y,
                        playerController.playerParts[10].targetRotation.z,
                        playerController.playerParts[10].targetRotation.w);
                    playerController.playerParts[7].targetRotation = new Quaternion(playerController.playerParts[7].targetRotation.x - 0.12f * playerController.stepHeight / 2,
                        playerController.playerParts[7].targetRotation.y,
                        playerController.playerParts[7].targetRotation.z,
                        playerController.playerParts[7].targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                if (playerController.walkBackward)
                {
                    playerController.playerParts[9].targetRotation = new Quaternion(playerController.playerParts[9].targetRotation.x - 0.00f * playerController.stepHeight,
                        playerController.playerParts[9].targetRotation.y,
                        playerController.playerParts[9].targetRotation.z,
                        playerController.playerParts[9].targetRotation.w);
                    playerController.playerParts[10].targetRotation = new Quaternion(playerController.playerParts[10].targetRotation.x - 0.07f * playerController.stepHeight * 2,
                        playerController.playerParts[10].targetRotation.y,
                        playerController.playerParts[10].targetRotation.z,
                        playerController.playerParts[10].targetRotation.w);
                    playerController.playerParts[7].targetRotation = new Quaternion(playerController.playerParts[7].targetRotation.x + 0.02f * playerController.stepHeight / 2,
                        playerController.playerParts[7].targetRotation.y,
                        playerController.playerParts[7].targetRotation.z,
                        playerController.playerParts[7].targetRotation.w);
                }

                //Checks if the stepTimerLeft is greater than stepDuration, and if so, it sets stepTimerLeft back to zero,
                //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                if (playerController.stepTimerLeft > playerController.stepDuration)
                {
                    ResetStepLeft();
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[9] and playerParts[10] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                playerController.playerParts[9].targetRotation = Quaternion.Lerp(playerController.playerParts[9].targetRotation,
                    playerController.UpperLeftLegTarget,
                    7f * Time.fixedDeltaTime);
                playerController.playerParts[10].targetRotation = Quaternion.Lerp(playerController.playerParts[10].targetRotation,
                    playerController.LowerLeftLegTarget,
                    18f * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                playerController.playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);
                playerController.playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * playerController.feetMountForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    public void ResetStepLeft()
    {
        playerController.stepTimerLeft = 0;

        playerController.stepLeft = false;

        if (playerController.walkForward || playerController.walkBackward)
        {
            playerController.stepRight = true;
        }
    }

    public void ResetStepRight()
    {
        playerController.stepTimerRight = 0;

        playerController.stepRight = false;

        if (playerController.walkForward || playerController.walkBackward)
        {
            playerController.stepLeft = true;
        }
    }

    public void ShouldMoveLegLeft()
    {
        playerController.stepLeft = true;
        playerController.alertLegLeft = true;
        playerController.alertLegRight = true;
    }

    public void ShouldMoveLegRight()
    {
        playerController.stepRight = true;
        playerController.alertLegRight = true;
        playerController.alertLegLeft = true;
    }
}
