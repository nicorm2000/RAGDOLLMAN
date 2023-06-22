using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Active Ragdoll Player parts
    [SerializeField]
    public GameObject Root,
        Body,
        Head,
        UpperRightArm,
        LowerRightArm,
        UpperLeftArm,
        LowerLeftArm,
        UpperRightLeg,
        LowerRightLeg,
        UpperLeftLeg,
        LowerLeftLeg,
        RightFoot,
        LeftFoot;

    //Center of mass point
    [SerializeField] public Transform COMP;

    //Rigidbody Hands
    [SerializeField]
    Rigidbody RightHand,
        LeftHand;

    [Header("Ground Dependancies")]
    [SerializeField] CheckerGround groundCheck;

    [Header("Ragdoll Activator Dependancies")]
    [SerializeField] ActivateRagdoll activateRagdoll;

    [Header("Ragdoll Deactivator Dependancies")]
    [SerializeField] DeactivateRagdoll deactivateRagdoll;

    [Header("Center of Mass Dependancies")]
    [SerializeField] CenterOfMass centerOfMass;

    [Header("Rotation Dependancies")]
    [SerializeField] PlayerRotation playerRotation;

    [Header("Reset Player's Pose Dependancies")]
    [SerializeField] ResetPlayerPose resetPlayerPose;

    [Header("Reset Walk Cycle Dependancies")]
    [SerializeField] ResetWalkCycle resetWalkCycle;

    [Header("Ragdoll Check Dependancies")]
    [SerializeField] RagdollCheck ragdollCheck;

    [Header("Player Setup Dependancies")]
    [SerializeField] PlayerSetup playerSetup;

    [Header("Step Prediction Dependancies")]
    [SerializeField] StepPrediction stepPrediction;

    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public HandContact grabRight,
        grabLeft;

    [Header("Input on this player")]
    //Enable controls
    public bool useControls = true;

    [Header("Player Input Axis")]
    //Player Axis controls
    public string forwardBackward = "Vertical",
        horizontal = "Horizontal",
        jump = "Jump",
        reachLeft = "Fire1",
        reachRight = "Fire2";

    [Header("The Layer Only This Player Is On")]
    //Player layer name
    public string thisPlayerLayer = "Player_1";

    [Header("Movement Properties")]
    //Movement
    public float moveSpeed = 10f,
        turnSpeed = 6f,
        jumpForce = 18f;

    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true,
        useStepPrediction = true;
    public float balanceHeight = 2.5f,
        balanceStrength = 5000f, coreStrength = 1500f,
        limbStrength = 500f;
    //Walking
    public float stepDuration = 0.2f,
        stepHeight = 1.7f,
        feetMountForce = 25f;

    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f,
        armReachStiffness = 2000f;

    //Hidden variables
    public float timer,
        step_Timer_Right,
        step_Timer_Left,
        MouseYAxisArms,
        MouseYAxisBody;

    public bool walkForward,
        walkBackward,
        stepRight,
        stepLeft,
        alert_Leg_Right,
        alert_Leg_Left,
        balanced = true,
        isKeyDown,
        moveAxisUsed,
        jumpAxisUsed,
        reachLeftAxisUsed,
        reachRightAxisUsed,
        jumping, isJumping,
        inAir,
        punchingRight,
        punchingLeft,
        isRagdoll,
        resetPose;

    public Camera cam;
    public Vector3 direction,
        centerOfMassPoint;

    //Player Parts Array
    public GameObject[] playerParts;

    //JointDrives in Unity are used to control the motion of joints in a physics simulation.
    //They allow for greater control over the movement of joints by allowing you to set target rotation and velocity values, along with positional spring and damping values.
    //This can help create realistic and stable movements in your physics simulations.
    //Joint Drives on & off
    public JointDrive BalanceOn,
        PoseOn,
        CoreStiffness,
        ReachStiffness,
        DriveOff;

    //Original pose target rotation
    public Quaternion HeadTarget,
        BodyTarget,
        UpperRightArmTarget,
        LowerRightArmTarget,
        UpperLeftArmTarget,
        LowerLeftArmTarget,
        UpperRightLegTarget,
        LowerRightLegTarget,
        UpperLeftLegTarget,
        LowerLeftLegTarget;

    private void Awake()
    {
        playerSetup.SetupPlayer();
    }

    private void Update()
    {
        if (useControls && !inAir)
        {
            PlayerMovement();
        }

        if (useControls)
        {
            PlayerReach();
        }

        if (balanced && useStepPrediction)
        {
            stepPrediction.PredictNextStep(ref walkForward, ref walkBackward, COMP, playerParts, isKeyDown, ref stepRight, ref stepLeft, ref alert_Leg_Right, ref alert_Leg_Left, ref step_Timer_Right, ref step_Timer_Left);
        }

        if (!useStepPrediction)
        {
            resetWalkCycle.WalkCycleReset(walkForward, walkBackward, ref stepRight, ref stepLeft, ref alert_Leg_Right, ref alert_Leg_Left, ref step_Timer_Right, ref step_Timer_Left);
        }

        groundCheck.GroundChecker(playerParts[0], balanceHeight, inAir, isJumping, reachRightAxisUsed, reachLeftAxisUsed, ref balanced, autoGetUpWhenPossible);

        ragdollCheck.RagdollChecker(balanced, isRagdoll);

        centerOfMass.CenterOfMassCalculation(COMP, centerOfMassPoint, playerParts);
    }

    private void FixedUpdate()
    {
        Walking();

        if (useControls)
        {
            playerRotation.PlayerRotationCalculation(cam, playerParts[0], turnSpeed);

            resetPlayerPose.PlayerPoseReset(ref resetPose, jumping, ref MouseYAxisArms, playerParts, BodyTarget, UpperRightArmTarget, LowerRightArmTarget, UpperLeftArmTarget, LowerLeftArmTarget);

            PlayerGetUpJumping();
        }
    }

    /// <summary>
    /// Handles the movement of the player character.
    /// </summary>
    private void PlayerMovement()
    {
        //Move in camera direction
        direction = playerParts[0].transform.rotation * new Vector3(Input.GetAxisRaw(horizontal), 0.0f, Input.GetAxisRaw(forwardBackward));

        direction.y = 0f;

        playerParts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(playerParts[0].transform.GetComponent<Rigidbody>().velocity, (direction * moveSpeed) + new Vector3(0, playerParts[0].transform.GetComponent<Rigidbody>().velocity.y, 0), 0.8f);

        //The player is balanced if this is true
        if (Input.GetAxisRaw(horizontal) != 0 || Input.GetAxisRaw(forwardBackward) != 0 && balanced)
        {
            if (!walkForward && !moveAxisUsed)
            {
                MovementTrue();
            }
        }
        else if (Input.GetAxisRaw(horizontal) == 0 && Input.GetAxisRaw(forwardBackward) == 0)
        {
            if (walkForward && moveAxisUsed)
            {
                MovementFalse();
            }
        }
    }

    private void MovementTrue()
    {
        walkForward = true;
        moveAxisUsed = true;
        isKeyDown = true;
    }

    private void MovementFalse()
    {
        walkForward = false;
        moveAxisUsed = false;
        isKeyDown = false;
    }

    /// <summary>
    /// Handles the player character's getting up and jumping actions.
    /// </summary>
    private void PlayerGetUpJumping()
    {
        if (Input.GetAxis(jump) > 0)
        {
            if (!jumpAxisUsed)
            {
                if (balanced && !inAir)
                {
                    jumping = true;
                }

                else if (!balanced)
                {
                    deactivateRagdoll.RagdollDeactivator(ref balanced, ref isRagdoll, reachRightAxisUsed, reachLeftAxisUsed, ref resetPose, playerParts, BalanceOn, PoseOn);
                }
            }

            jumpAxisUsed = true;
        }
        else
        {
            jumpAxisUsed = false;
        }

        //Calculates the jump force by multiplying the transform.up vector of the Rigidbody attached to playerParts[0] by jumpForce,
        //and sets the x and z values of the vector to the respective values of the current velocity of the Rigidbody.
        if (jumping)
        {
            isJumping = true;

            var v3 = playerParts[0].GetComponent<Rigidbody>().transform.up * jumpForce;

            v3.x = playerParts[0].GetComponent<Rigidbody>().velocity.x;

            v3.z = playerParts[0].GetComponent<Rigidbody>().velocity.z;

            playerParts[0].GetComponent<Rigidbody>().velocity = v3;
        }

        if (isJumping)
        {
            timer = timer + Time.fixedDeltaTime;

            if (timer > 0.2f)
            {
                timer = 0.0f;

                jumping = false;
                isJumping = false;
                inAir = true;
            }
        }
    }

    /// <summary>
    /// Handles the player's reach with both hands.
    /// </summary>
    private void PlayerReach()
    {
        //Simulates body bending by allowing the player to move the mouse up and down to control the rotation of a ConfigurableJoint component attached to playerParts[1]
        if (1 == 1)
        {
            if (MouseYAxisBody <= 0.9f && MouseYAxisBody >= -0.9f)
            {
                MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            else if (MouseYAxisBody > 0.9f)
            {
                MouseYAxisBody = 0.9f;
            }
            else if (MouseYAxisBody < -0.9f)
            {
                MouseYAxisBody = -0.9f;
            }

            //Set the targetRotation of the ConfigurableJoint attached to playerParts[1] to a new quaternion that represents the rotation of the torso. 
            playerParts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }

        //Handle player's left reach
        if (Input.GetAxisRaw(reachLeft) != 0)
        {
            if (!reachLeftAxisUsed)
            {
                //Adjust Left Arm joint strength
                playerParts[5].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                playerParts[5].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                playerParts[6].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                playerParts[6].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                playerParts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                playerParts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

                reachLeftAxisUsed = true;
            }

            if (MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            else if (MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            else if (MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }

            //Upper  left arm pose
            playerParts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.58f - (MouseYAxisArms), -0.88f - (MouseYAxisArms), -0.8f, 1);
        }

        if (Input.GetAxisRaw(reachLeft) == 0)
        {
            //Sets the left reach input is not being used (specified by the reachLeft variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to playerParts[5], playerParts[6], and playerParts[1]
            if (reachLeftAxisUsed)
            {
                if (balanced)
                {
                    playerParts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    playerParts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

                    playerParts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    playerParts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    playerParts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    playerParts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    playerParts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }

                resetPose = true;
                reachLeftAxisUsed = false;
            }
        }

        //Handle player's right reach
        if (Input.GetAxisRaw(reachRight) != 0)
        {
            if (!reachRightAxisUsed)
            {
                //Adjust Right Arm joint strength
                playerParts[3].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                playerParts[3].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                playerParts[4].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                playerParts[4].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                playerParts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                playerParts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

                reachRightAxisUsed = true;
            }

            if (MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            else if (MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            else if (MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }

            //Upper right arm pose
            playerParts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.58f + (MouseYAxisArms), -0.88f - (MouseYAxisArms), 0.8f, 1);
        }

        if (Input.GetAxisRaw(reachRight) == 0)
        {
            //Sets the right reach input is not being used (specified by the reachRight variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to playerParts[3], playerParts[4], and playerParts[1]
            if (reachRightAxisUsed)
            {
                if (balanced)
                {
                    playerParts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    playerParts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

                    playerParts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    playerParts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    playerParts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    playerParts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    playerParts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    playerParts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }

                resetPose = true;
                reachRightAxisUsed = false;
            }
        }
    }

    /// <summary>
    /// Handles the player's walking.
    /// </summary>
    private void Walking()
    {
        if (!inAir)
        {
            if (walkForward)
            {
                //Checks if the right leg is behind
                if (playerParts[11].transform.position.z < playerParts[12].transform.position.z && !stepLeft && !alert_Leg_Right)
                {
                    stepRight = true;
                    alert_Leg_Right = true;
                    alert_Leg_Left = true;
                }

                //Checks if the left leg is behind
                if (playerParts[11].transform.position.z > playerParts[12].transform.position.z && !stepRight && !alert_Leg_Left)
                {
                    stepLeft = true;
                    alert_Leg_Left = true;
                    alert_Leg_Right = true;
                }
            }

            if (walkBackward)
            {
                //Checks if the right leg is ahead
                if (playerParts[11].transform.position.z > playerParts[12].transform.position.z && !stepLeft && !alert_Leg_Right)
                {
                    stepRight = true;
                    alert_Leg_Right = true;
                    alert_Leg_Left = true;
                }

                //Checks if the left leg is ahead
                if (playerParts[11].transform.position.z < playerParts[12].transform.position.z && !stepRight && !alert_Leg_Left)
                {
                    stepLeft = true;
                    alert_Leg_Left = true;
                    alert_Leg_Right = true;
                }
            }

            //step right
            if (stepRight)
            {
                //Use fixedDeltaTime because of physics operations
                step_Timer_Right += Time.fixedDeltaTime;

                //Right foot force down
                playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                if (walkForward)
                {
                    playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * stepHeight, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * stepHeight * 2, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[9].GetComponent<ConfigurableJoint>().GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * stepHeight / 2, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[8], and playerParts[9] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                if (walkBackward)
                {
                    playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * stepHeight, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * stepHeight * 2, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * stepHeight / 2, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Checks if the step_R_timer is greater than stepDuration, and if so, it sets step_R_timer back to zero,
                //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                if (step_Timer_Right > stepDuration)
                {
                    step_Timer_Right = 0;

                    stepRight = false;

                    if (walkForward || walkBackward)
                    {
                        stepLeft = true;
                    }
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[7] and playerParts[8] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
                playerParts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
            }


            //step left
            if (stepLeft)
            {
                //Use fixedDeltaTime because of physics operations
                step_Timer_Left += Time.fixedDeltaTime;

                //Left foot force down
                playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by stepHeight.
                if (walkForward)
                {
                    playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * stepHeight, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * stepHeight * 2, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * stepHeight / 2, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //playerParts[7], playerParts[9], and playerParts[10] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by stepHeight.
                if (walkBackward)
                {
                    playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * stepHeight, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * stepHeight * 2, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    playerParts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * stepHeight / 2, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.y, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.z, playerParts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Checks if the step_R_timer is greater than stepDuration, and if so, it sets step_R_timer back to zero,
                //sets stepRight to false, and sets stepLeft to true if walkForward or walkBackward are true.
                if (step_Timer_Left > stepDuration)
                {
                    step_Timer_Left = 0;

                    stepLeft = false;

                    if (walkForward || walkBackward)
                    {
                        stepRight = true;
                    }
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to playerParts[9] and playerParts[10] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                playerParts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
                playerParts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(playerParts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to playerParts[11] and playerParts[12]
                //with a negative Vector3.up value, scaled by feetMountForce and Time.deltaTime.
                playerParts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
                playerParts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * feetMountForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}