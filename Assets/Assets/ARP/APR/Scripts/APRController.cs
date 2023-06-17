using UnityEngine;

public class APRController : MonoBehaviour
{
    //Active Ragdoll Player parts
    [SerializeField] GameObject Root, Body, Head, UpperRightArm, LowerRightArm, UpperLeftArm, LowerLeftArm, UpperRightLeg, LowerRightLeg, UpperLeftLeg, LowerLeftLeg, RightFoot, LeftFoot;

    //Rigidbody Hands
    [SerializeField] Rigidbody RightHand, LeftHand;

    //Center of mass point
    [SerializeField] Transform COMP;

    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public HandContact GrabRight;
    public HandContact GrabLeft;

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
    public bool useStepPrediction = true;
    public float balanceHeight = 2.5f;
    public float balanceStrength = 5000f;
    public float coreStrength = 1500f;
    public float limbStrength = 500f;
    //Walking
    public float StepDuration = 0.2f;
    public float StepHeight = 1.7f;
    public float FeetMountForce = 25f;

    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;

    //Hidden variables
    private float timer, Step_R_timer, Step_L_timer, MouseYAxisArms, MouseYAxisBody;

    private bool WalkForward, WalkBackward, StepRight, StepLeft, Alert_Leg_Right, Alert_Leg_Left, balanced = true, ResetPose, isRagdoll, isKeyDown, moveAxisUsed, jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;

    public bool jumping, isJumping, inAir, punchingRight, punchingLeft;

    private Camera cam;
    private Vector3 Direction;
    private Vector3 CenterOfMassPoint;

    //Active Ragdoll Player Parts Array
    private GameObject[] APR_Parts;

    //JointDrives in Unity are used to control the motion of joints in a physics simulation.
    //They allow for greater control over the movement of joints by allowing you to set target rotation and velocity values, along with positional spring and damping values.
    //This can help create realistic and stable movements in your physics simulations.
    //Joint Drives on & off
    JointDrive BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;

    //Original pose target rotation
    Quaternion HeadTarget, BodyTarget, UpperRightArmTarget, LowerRightArmTarget, UpperLeftArmTarget, LowerLeftArmTarget, UpperRightLegTarget, LowerRightLegTarget, UpperLeftLegTarget, LowerLeftLegTarget;

    [Header("Player Editor Debug Mode")]
    //Debug
    public bool editorDebugMode;

    void Awake()
    {
        PlayerSetup();
    }

    void Update()
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
            StepPrediction();
            CenterOfMass();
        }

        if (!useStepPrediction)
        {
            ResetWalkCycle();
        }

        GroundCheck();
        CenterOfMass();
    }

    void FixedUpdate()
    {
        Walking();

        if (useControls)
        {
            PlayerRotation();
            ResetPlayerPose();

            PlayerGetUpJumping();
        }
    }

    /// <summary>
    /// Set up the player character by initializing various variables and configuring joint drives.
    /// </summary>
    void PlayerSetup()
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
        APR_Parts = new GameObject[] { Root, Body, Head, UpperRightArm, LowerRightArm, UpperLeftArm, LowerLeftArm, UpperRightLeg, LowerRightLeg, UpperLeftLeg, LowerLeftLeg, RightFoot, LeftFoot };

        //Setup original pose for joint drives
        BodyTarget = APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation;
        HeadTarget = APR_Parts[2].GetComponent<ConfigurableJoint>().targetRotation;
        UpperRightArmTarget = APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightArmTarget = APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftArmTarget = APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftArmTarget = APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation;
        UpperRightLegTarget = APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation;
        LowerRightLegTarget = APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation;
        UpperLeftLegTarget = APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation;
        LowerLeftLegTarget = APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation;
    }

    /// <summary>
    /// Check if the player character is in contact with the ground and adjust balancing accordingly.
    /// </summary>
    void GroundCheck()
    {
        Ray ray = new Ray(APR_Parts[0].transform.position, -APR_Parts[0].transform.up);

        RaycastHit hit;

        //Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            if (!balanced && APR_Parts[0].GetComponent<Rigidbody>().velocity.magnitude < 1f)
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

        //Balance on/off
        if (balanced && isRagdoll)
        {
            DeactivateRagdoll();
        }
        else if (!balanced && !isRagdoll)
        {
            ActivateRagdoll();
        }
    }

    /// <summary>
    /// Predicts the stepping behavior based on the character's balance state and movement direction.
    /// </summary>
    void StepPrediction()
    {
        //Reset variables when balanced
        if (!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }

        //Check direction to walk when off balance
        //Walk backward
        if (COMP.position.z < APR_Parts[11].transform.position.z && COMP.position.z < APR_Parts[12].transform.position.z)
        {
            WalkBackward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                WalkBackward = false;
            }
        }

        //Walk forward
        if (COMP.position.z > APR_Parts[11].transform.position.z && COMP.position.z > APR_Parts[12].transform.position.z)
        {
            WalkForward = true;
        }
        else
        {
            if (!isKeyDown)
            {
                WalkForward = false;
            }
        }
    }

    /// <summary>
    /// Resets the walk cycle variables when the character is not moving.
    /// </summary>
    void ResetWalkCycle()
    {
        //Reset variables when not moving
        if (!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
    }

    /// <summary>
    /// Handles the movement of the player character.
    /// </summary>
    void PlayerMovement()
    {
        //Move in camera direction
        Direction = APR_Parts[0].transform.rotation * new Vector3(Input.GetAxisRaw(horizontal), 0.0f, Input.GetAxisRaw(forwardBackward));

        Direction.y = 0f;

        APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * moveSpeed) + new Vector3(0, APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0), 0.8f);

        //The player is balanced if this is true
        if (Input.GetAxisRaw(horizontal) != 0 || Input.GetAxisRaw(forwardBackward) != 0 && balanced)
        {
            if (!WalkForward && !moveAxisUsed)
            {
                WalkForward = true;
                moveAxisUsed = true;
                isKeyDown = true;
            }
        }
        else if (Input.GetAxisRaw(horizontal) == 0 && Input.GetAxisRaw(forwardBackward) == 0)
        {
            if (WalkForward && moveAxisUsed)
            {
                WalkForward = false;
                moveAxisUsed = false;
                isKeyDown = false;
            }
        }
    }

    /// <summary>
    /// Handles the rotation of the player character.
    /// </summary>
    void PlayerRotation()
    {
        //Camera Direction and turn of camera.
        var lookPos = cam.transform.forward;

        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        //This allows for a smooth rotation of the character.
        APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Slerp(APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation, Quaternion.Inverse(rotation), Time.deltaTime * turnSpeed);
    }

    /// <summary>
    /// Handles the player character's getting up and jumping actions.
    /// </summary>
    void PlayerGetUpJumping()
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

        //Calculates the jump force by multiplying the transform.up vector of the Rigidbody attached to APR_Parts[0] by jumpForce,
        //and sets the x and z values of the vector to the respective values of the current velocity of the Rigidbody.
        if (jumping)
        {
            isJumping = true;

            var v3 = APR_Parts[0].GetComponent<Rigidbody>().transform.up * jumpForce;

            v3.x = APR_Parts[0].GetComponent<Rigidbody>().velocity.x;

            v3.z = APR_Parts[0].GetComponent<Rigidbody>().velocity.z;

            APR_Parts[0].GetComponent<Rigidbody>().velocity = v3;
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
    /// Handles the player character's landing after being in the air.
    /// </summary>
    public void PlayerLanded()
    {
        if (inAir && !isJumping && !jumping)
        {
            inAir = false;
            ResetPose = true;
        }
    }

    /// <summary>
    /// Handles the player's reach with both hands.
    /// </summary>
    void PlayerReach()
    {
        //Simulates body bending by allowing the player to move the mouse up and down to control the rotation of a ConfigurableJoint component attached to APR_Parts[1]
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

            //Set the targetRotation of the ConfigurableJoint attached to APR_Parts[1] to a new quaternion that represents the rotation of the torso. 
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }

        //Handle player's left reach
        if (Input.GetAxisRaw(reachLeft) != 0)
        {
            if (!reachLeftAxisUsed)
            {
                //Adjust Left Arm joint strength
                APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

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
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.58f - (MouseYAxisArms), -0.88f - (MouseYAxisArms), -0.8f, 1);
        }

        if (Input.GetAxisRaw(reachLeft) == 0)
        {
            //Sets the left reach input is not being used (specified by the reachLeft variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to APR_Parts[5], APR_Parts[6], and APR_Parts[1]
            if (reachLeftAxisUsed)
            {
                if (balanced)
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

                    APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }

                ResetPose = true;
                reachLeftAxisUsed = false;
            }
        }

        //Handle player's right reach
        if (Input.GetAxisRaw(reachRight) != 0)
        {
            if (!reachRightAxisUsed)
            {
                //Adjust Right Arm joint strength
                APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
                APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
                APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
                APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

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
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.58f + (MouseYAxisArms), -0.88f - (MouseYAxisArms), 0.8f, 1);
        }

        if (Input.GetAxisRaw(reachRight) == 0)
        {
            //Sets the right reach input is not being used (specified by the reachRight variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to APR_Parts[3], APR_Parts[4], and APR_Parts[1]
            if (reachRightAxisUsed)
            {
                if (balanced)
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

                    APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
                    APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
                    APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
                }

                ResetPose = true;
                reachRightAxisUsed = false;
            }
        }
    }

    /// <summary>
    /// Handles the player's walking.
    /// </summary>
    void Walking()
    {
        if (!inAir)
        {
            if (WalkForward)
            {
                //Checks if the right leg is behind
                if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
                {
                    StepRight = true;
                    Alert_Leg_Right = true;
                    Alert_Leg_Left = true;
                }

                //Checks if the left leg is behind
                if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
                {
                    StepLeft = true;
                    Alert_Leg_Left = true;
                    Alert_Leg_Right = true;
                }
            }

            if (WalkBackward)
            {
                //Checks if the right leg is ahead
                if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
                {
                    StepRight = true;
                    Alert_Leg_Right = true;
                    Alert_Leg_Left = true;
                }

                //Checks if the left leg is ahead
                if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
                {
                    StepLeft = true;
                    Alert_Leg_Left = true;
                    Alert_Leg_Right = true;
                }
            }

            //Step right
            if (StepRight)
            {
                //Use fixedDeltaTime because of physics operations
                Step_R_timer += Time.fixedDeltaTime;

                //Right foot force down
                APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //APR_Parts[7], APR_Parts[8], and APR_Parts[9] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by StepHeight.
                if (WalkForward)
                {
                    APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[9].GetComponent<ConfigurableJoint>().GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //APR_Parts[7], APR_Parts[8], and APR_Parts[9] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by StepHeight.
                if (WalkBackward)
                {
                    APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Checks if the Step_R_timer is greater than StepDuration, and if so, it sets Step_R_timer back to zero,
                //sets StepRight to false, and sets StepLeft to true if WalkForward or WalkBackward are true.
                if (Step_R_timer > StepDuration)
                {
                    Step_R_timer = 0;

                    StepRight = false;

                    if (WalkForward || WalkBackward)
                    {
                        StepLeft = true;
                    }
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to APR_Parts[7] and APR_Parts[8] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
                APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to APR_Parts[11] and APR_Parts[12]
                //with a negative Vector3.up value, scaled by FeetMountForce and Time.deltaTime.
                APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            }


            //Step left
            if (StepLeft)
            {
                //Use fixedDeltaTime because of physics operations
                Step_L_timer += Time.fixedDeltaTime;

                //Left foot force down
                APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //APR_Parts[7], APR_Parts[9], and APR_Parts[10] to quaternion values,
                //whose x, y, and z component values are gradually incremented or decremented by constants scaled by StepHeight.
                if (WalkForward)
                {
                    APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Simulates the walking motion of the character by setting the targetRotation of the ConfigurableJoint components attached to
                //APR_Parts[7], APR_Parts[9], and APR_Parts[10] to quaternion values,
                //whose x, y, and z component values are multiplied or divided by constants scaled by StepHeight.
                if (WalkBackward)
                {
                    APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);
                    APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
                }

                //Checks if the Step_R_timer is greater than StepDuration, and if so, it sets Step_R_timer back to zero,
                //sets StepRight to false, and sets StepLeft to true if WalkForward or WalkBackward are true.
                if (Step_L_timer > StepDuration)
                {
                    Step_L_timer = 0;

                    StepLeft = false;

                    if (WalkForward || WalkBackward)
                    {
                        StepRight = true;
                    }
                }
            }
            else
            {
                //Reset to idle
                //Resets the targetRotation of the ConfigurableJoint components attached to APR_Parts[9] and APR_Parts[10] to quaternion values,
                //that gradually interpolate back to their default values, scaled by specified time deltas. 
                APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
                APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

                //Simulates the feet being firmly planted on the ground by calling AddForce() on the Rigidbody components attached to APR_Parts[11] and APR_Parts[12]
                //with a negative Vector3.up value, scaled by FeetMountForce and Time.deltaTime.
                APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
                APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// Activates player raagdoll, which controls balance.
    /// </summary>
    public void ActivateRagdoll()
    {
        //To start being a raggdoll the balance and pose are set OFF, by using DriveOff.

        isRagdoll = true;
        balanced = false;

        //Root
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        //Head
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;

        //Arms
        if (!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        if (!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        }

        //Legs
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
    }

    /// <summary>
    /// Deactivates player raagdoll, which controls balance.
    /// </summary>
    void DeactivateRagdoll()
    {
        //To stop being a raggdoll the balance and pose are set ON.

        isRagdoll = false;
        balanced = true;

        //Root
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = BalanceOn;
        APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = BalanceOn;
        //Head
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

        //Arms
        if (!reachRightAxisUsed)
        {
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        if (!reachLeftAxisUsed)
        {
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
            APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        }

        //Legs
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
        APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

        ResetPose = true;
    }

    /// <summary>
    /// Resets player pose to original, which is balanced.
    /// </summary>
    void ResetPlayerPose()
    {
        if (ResetPose && !jumping)
        {
            APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
            APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
            APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
            APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
            APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

            MouseYAxisArms = 0;

            ResetPose = false;
        }
    }

    /// <summary>
    /// Character's center of mass.
    /// </summary>
    void CenterOfMass()
    {
        //Calculates the center of mass of a rigid body by adding up the positions of the colliders weighted by their mass,
        //and then dividing by the total mass of the rigid body.
        //The result is the center of mass point of the entire rigid body.
        //Formula -> CenterOfMassPoint = (m1 * p1 + m2 * p2 + ... + mn * pn) / (m1 + m2 + ... + mn)
        //m is the mass of each collider, and p is the position of each collider

        CenterOfMassPoint =
            (APR_Parts[0].GetComponent<Rigidbody>().mass * APR_Parts[0].transform.position +
             APR_Parts[1].GetComponent<Rigidbody>().mass * APR_Parts[1].transform.position +
             APR_Parts[2].GetComponent<Rigidbody>().mass * APR_Parts[2].transform.position +
             APR_Parts[3].GetComponent<Rigidbody>().mass * APR_Parts[3].transform.position +
             APR_Parts[4].GetComponent<Rigidbody>().mass * APR_Parts[4].transform.position +
             APR_Parts[5].GetComponent<Rigidbody>().mass * APR_Parts[5].transform.position +
             APR_Parts[6].GetComponent<Rigidbody>().mass * APR_Parts[6].transform.position +
             APR_Parts[7].GetComponent<Rigidbody>().mass * APR_Parts[7].transform.position +
             APR_Parts[8].GetComponent<Rigidbody>().mass * APR_Parts[8].transform.position +
             APR_Parts[9].GetComponent<Rigidbody>().mass * APR_Parts[9].transform.position +
             APR_Parts[10].GetComponent<Rigidbody>().mass * APR_Parts[10].transform.position +
             APR_Parts[11].GetComponent<Rigidbody>().mass * APR_Parts[11].transform.position +
             APR_Parts[12].GetComponent<Rigidbody>().mass * APR_Parts[12].transform.position)
            /
            (APR_Parts[0].GetComponent<Rigidbody>().mass + APR_Parts[1].GetComponent<Rigidbody>().mass +
             APR_Parts[2].GetComponent<Rigidbody>().mass + APR_Parts[3].GetComponent<Rigidbody>().mass +
             APR_Parts[4].GetComponent<Rigidbody>().mass + APR_Parts[5].GetComponent<Rigidbody>().mass +
             APR_Parts[6].GetComponent<Rigidbody>().mass + APR_Parts[7].GetComponent<Rigidbody>().mass +
             APR_Parts[8].GetComponent<Rigidbody>().mass + APR_Parts[9].GetComponent<Rigidbody>().mass +
             APR_Parts[10].GetComponent<Rigidbody>().mass + APR_Parts[11].GetComponent<Rigidbody>().mass +
             APR_Parts[12].GetComponent<Rigidbody>().mass);

        COMP.position = CenterOfMassPoint;
    }
}