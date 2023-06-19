using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Active Ragdoll Player parts
    [SerializeField] GameObject Root, Body, Head, UpperRightArm, LowerRightArm, UpperLeftArm, LowerLeftArm, UpperRightLeg, LowerRightLeg, UpperLeftLeg, LowerLeftLeg, RightFoot, LeftFoot;

    //Rigidbody Hands
    [SerializeField] Rigidbody RightHand, LeftHand;

    //Center of mass point
    [SerializeField] Transform COMP;

    [Header("Ground Dependancies")]
    [SerializeField] GroundCheck groundCheck;

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

    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public HandContact grabRight;
    public HandContact grabLeft;

    [Header("Input on this player")]
    //Enable controls
    public bool useControls = true;

    [Header("Player Input Axis")]
    //Player Axis controls
    public string forwardBackward = "Vertical";
    public string horizontal = "Horizontal";
    public string jump = "Jump";
    public string reachLeft = "Fire1";
    public string reachRight = "Fire2";

    [Header("The Layer Only This Player Is On")]
    //Player layer name
    public string thisPlayerLayer = "Player_1";

    [Header("Movement Properties")]
    //Movement
    public float moveSpeed = 10f;
    public float turnSpeed = 6f;
    public float jumpForce = 18f;

    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true;
    public bool usestepPrediction = true;
    public float balanceHeight = 2.5f;
    public float balanceStrength = 5000f;
    public float coreStrength = 1500f;
    public float limbStrength = 500f;
    //Walking
    public float stepDuration = 0.2f;
    public float stepHeight = 1.7f;
    public float feetMountForce = 25f;

    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;

    //Hidden variables
    public float timer, step_R_timer, step_L_timer, MouseYAxisArms, MouseYAxisBody;

    private bool walkForward, walkBackward, stepRight, stepLeft, alert_Leg_Right, alert_Leg_Left, balanced = true, isKeyDown, moveAxisUsed, jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;

    public bool jumping, isJumping, inAir, punchingRight, punchingLeft, isRagdoll, resetPose;

    private Camera cam;
    private Vector3 direction;
    private Vector3 centerOfMassPoint;

    //Player Parts Array
    private GameObject[] playerParts;

    //JointDrives in Unity are used to control the motion of joints in a physics simulation.
    //They allow for greater control over the movement of joints by allowing you to set target rotation and velocity values, along with positional spring and damping values.
    //This can help create realistic and stable movements in your physics simulations.
    //Joint Drives on & off
    JointDrive BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;

    //Original pose target rotation
    Quaternion HeadTarget, BodyTarget, UpperRightArmTarget, LowerRightArmTarget, UpperLeftArmTarget, LowerLeftArmTarget, UpperRightLegTarget, LowerRightLegTarget, UpperLeftLegTarget, LowerLeftLegTarget;

    private void Awake()
    {
        PlayerSetup();
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

        if (balanced && usestepPrediction)
        {
            StepPrediction();
        }

        if (!usestepPrediction)
        {
            ResetWalkCycle();
            //resetWalkCycle.WalkCycleReset(walkForward, walkBackward, stepRight, stepLeft, alert_Leg_Right, alert_Leg_Left, step_R_timer, step_L_timer);
        }

        GroundCheck();
        //groundCheck.GroundChecker(playerParts[0], balanceHeight, inAir, isJumping, reachRightAxisUsed, reachLeftAxisUsed, balanced, autoGetUpWhenPossible);
        RagdollCheck();
        //CenterOfMass();
        centerOfMass.CenterOfMassCalculation(COMP, centerOfMassPoint, playerParts);
    }

    private void FixedUpdate()
    {
        Walking();

        if (useControls)
        {
            playerRotation.PlayerRotationCalculation(cam, playerParts[0], turnSpeed);
            ResetPlayerPose();
            //resetPlayerPose.PlayerPoseReset(resetPose, jumping, MouseYAxisArms, playerParts, BodyTarget, UpperRightArmTarget, LowerRightArmTarget, UpperLeftArmTarget, LowerLeftArmTarget);
            PlayerGetUpJumping();
        }
    }

    /// <summary>
    /// Set up the player character by initializing various variables and configuring joint drives.
    /// </summary>
    private void PlayerSetup()
    {
        cam = Camera.main;

        //Setup joint drives
        BalanceOn = new JointDrive();
        BalanceOn.positionSpring = balanceStrength;
        BalanceOn.positionDamper = 0;
        BalanceOn.maximumForce = Mathf.Infinity;

        PoseOn = new JointDrive();
        PoseOn.positionSpring = limbStrength;
        PoseOn.positionDamper = 0;
        PoseOn.maximumForce = Mathf.Infinity;

        CoreStiffness = new JointDrive();
        CoreStiffness.positionSpring = coreStrength;
        CoreStiffness.positionDamper = 0;
        CoreStiffness.maximumForce = Mathf.Infinity;

        ReachStiffness = new JointDrive();
        ReachStiffness.positionSpring = armReachStiffness;
        ReachStiffness.positionDamper = 0;
        ReachStiffness.maximumForce = Mathf.Infinity;

        DriveOff = new JointDrive();
        DriveOff.positionSpring = 25;
        DriveOff.positionDamper = 0;
        DriveOff.maximumForce = Mathf.Infinity;

        //Setup/reroute active ragdoll parts to array
        playerParts = new GameObject[] { Root, Body, Head, UpperRightArm, LowerRightArm, UpperLeftArm, LowerLeftArm, UpperRightLeg, LowerRightLeg, UpperLeftLeg, LowerLeftLeg, RightFoot, LeftFoot };

        //Setup original pose for joint drives
        BodyTarget = playerParts[1].GetComponent<ConfigurableJoint>().targetRotation;
        HeadTarget = playerParts[2].GetComponent<ConfigurableJoint>().targetRotation;
        UpperRightArmTarget = playerParts[3].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightArmTarget = playerParts[4].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftArmTarget = playerParts[5].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftArmTarget = playerParts[6].GetComponent<ConfigurableJoint>().targetRotation;
        UpperRightLegTarget = playerParts[7].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightLegTarget = playerParts[8].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftLegTarget = playerParts[9].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftLegTarget = playerParts[10].GetComponent<ConfigurableJoint>().targetRotation;
    }

    /// <summary>
    /// Check if the player character is in contact with the ground and adjust balancing accordingly.
    /// </summary>
    private void GroundCheck()
    {
        Ray ray = new Ray(playerParts[0].transform.position, -playerParts[0].transform.up);

        RaycastHit hit;

        //Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            if (!balanced && playerParts[0].GetComponent<Rigidbody>().velocity.magnitude < 1f)
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

    /// <summary>
    /// Check the ragdoll state, balanced or not, so that it can be changed.
    /// </summary>
    private void RagdollCheck()
    {
        //Balance on/off
        if (balanced && isRagdoll)
        {
            DeactivateRagdoll();
            //deactivateRagdoll.RagdollDeactivator(balanced, isRagdoll, reachRightAxisUsed, reachLeftAxisUsed, resetPose, playerParts[0], playerParts[2], playerParts[3], playerParts[4], playerParts[5], playerParts[6], playerParts[7], playerParts[8], playerParts[9], playerParts[10], playerParts[11], playerParts[12], BalanceOn, PoseOn);
        }
        else if (!balanced && !isRagdoll)
        {
            ActivateRagdoll();
            //activateRagdoll.RagdollActivator(balanced, isRagdoll, reachRightAxisUsed, reachLeftAxisUsed, playerParts[0], playerParts[2], playerParts[3], playerParts[4], playerParts[5], playerParts[6], playerParts[7], playerParts[8], playerParts[9], playerParts[10], playerParts[11], playerParts[12], DriveOff);
        }
    }

    /// <summary>
    /// Predicts the stepping behavior based on the character's balance state and movement direction.
    /// </summary>
    private void StepPrediction()
    {
        //Reset variables when balanced
        if (!walkForward && !walkBackward)
        {
            ResetWalkCycle();
            //resetWalkCycle.WalkCycleReset(walkForward, walkBackward, stepRight, stepLeft, alert_Leg_Right, alert_Leg_Left, step_R_timer, step_L_timer);
        }

        //Check direction to walk when off balance
        //Walk backward
        if (COMP.position.z < playerParts[11].transform.position.z && COMP.position.z < playerParts[12].transform.position.z)
        {
            walkBackward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                walkBackward = false;
            }
        }

        //Walk forward
        if (COMP.position.z > playerParts[11].transform.position.z && COMP.position.z > playerParts[12].transform.position.z)
        {
            walkForward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                walkForward = false;
            }
        }
    }

    /// <summary>
    /// Resets the walk cycle variables when the character is not moving.
    /// </summary>
    private void ResetWalkCycle()
    {
        //Reset variables when not moving
        if (!walkForward && !walkBackward)
        {
            stepRight = false;
            stepLeft = false;
            step_R_timer = 0;
            step_L_timer = 0;
            alert_Leg_Right = false;
            alert_Leg_Left = false;
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
                    DeactivateRagdoll();
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
                step_R_timer += Time.fixedDeltaTime;

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
                if (step_R_timer > stepDuration)
                {
                    step_R_timer = 0;

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
                step_L_timer += Time.fixedDeltaTime;

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
                if (step_L_timer > stepDuration)
                {
                    step_L_timer = 0;

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

    /// <summary>
    /// Activates player raagdoll, which controls balance.
    /// </summary>
    private void ActivateRagdoll()
    {
        //To start being a raggdoll the balance and pose are set OFF, by using DriveOff.

        isRagdoll = true;
        balanced = false;

        //Root
        playerParts[0].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[0].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        //Head
        playerParts[2].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[2].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;

        //Arms
        if (!reachRightAxisUsed)
        {
            playerParts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            playerParts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            playerParts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            playerParts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        if (!reachLeftAxisUsed)
        {
            playerParts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            playerParts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            playerParts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            playerParts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        //Legs
        playerParts[7].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[7].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        playerParts[8].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[8].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        playerParts[9].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[9].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        playerParts[10].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[10].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        playerParts[11].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[11].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        playerParts[12].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        playerParts[12].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
    }

    /// <summary>
    /// Deactivates player raagdoll, which controls balance.
    /// </summary>
    private void DeactivateRagdoll()
    {
        //To stop being a raggdoll the balance and pose are set ON.

        isRagdoll = false;
        balanced = true;

        //Root
        playerParts[0].GetComponent<ConfigurableJoint>().angularXDrive = BalanceOn;
        playerParts[0].GetComponent<ConfigurableJoint>().angularYZDrive = BalanceOn;
        //Head
        playerParts[2].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[2].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

        //Arms
        if (!reachRightAxisUsed)
        {
            playerParts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            playerParts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            playerParts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            playerParts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        if (!reachLeftAxisUsed)
        {
            playerParts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            playerParts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            playerParts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            playerParts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        //Legs
        playerParts[7].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[7].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        playerParts[8].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[8].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        playerParts[9].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[9].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        playerParts[10].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[10].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        playerParts[11].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[11].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        playerParts[12].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        playerParts[12].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

        resetPose = true;
    }

    /// <summary>
    /// Resets player pose to original, which is balanced.
    /// </summary>
    private void ResetPlayerPose()
    {
        if (resetPose && !jumping)
        {
            playerParts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
            playerParts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
            playerParts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
            playerParts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
            playerParts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

            MouseYAxisArms = 0;

            resetPose = false;
        }
    }

    /// <summary>
    /// Handles the player character's landing after being in the air.
    /// </summary>
    public void PlayerLanded()
    {
        bool playerLanded = inAir && !isJumping && !jumping;

        if (playerLanded)
        {
            inAir = false;
            resetPose = true;
        }
    }
}