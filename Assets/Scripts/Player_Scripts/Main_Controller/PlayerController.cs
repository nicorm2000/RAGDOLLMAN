using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Active Ragdoll Player parts
    public ConfigurableJoint Root;
    public ConfigurableJoint Body;
    public ConfigurableJoint Head;
    public ConfigurableJoint UpperRightArm;
    public ConfigurableJoint LowerRightArm;
    public ConfigurableJoint UpperLeftArm;
    public ConfigurableJoint LowerLeftArm;
    public ConfigurableJoint UpperRightLeg;
    public ConfigurableJoint LowerRightLeg;
    public ConfigurableJoint UpperLeftLeg;
    public ConfigurableJoint LowerLeftLeg;
    public ConfigurableJoint RightFoot;
    public ConfigurableJoint LeftFoot;

    //Center of mass point
    public Transform COMP;

    [Header("Ground Dependancies")]
    [SerializeField] private CheckerGround groundCheck;

    [Header("Ragdoll Activator Dependancies")]
    [SerializeField] private ActivateRagdoll activateRagdoll;

    [Header("Ragdoll Deactivator Dependancies")]
    [SerializeField] private DeactivateRagdoll deactivateRagdoll;

    [Header("Center of Mass Dependancies")]
    [SerializeField] private CenterOfMass centerOfMass;

    [Header("Rotation Dependancies")]
    [SerializeField] private PlayerRotation playerRotation;

    [Header("Reset Player's Pose Dependancies")]
    [SerializeField] private ResetPlayerPose resetPlayerPose;

    [Header("Reset Walk Cycle Dependancies")]
    [SerializeField] private ResetWalkCycle resetWalkCycle;

    [Header("Ragdoll Check Dependancies")]
    [SerializeField] private RagdollCheck ragdollCheck;

    [Header("Player Setup Dependancies")]
    [SerializeField] private PlayerSetup playerSetup;

    [Header("Step Prediction Dependancies")]
    [SerializeField] StepPrediction stepPrediction;
    
    [Header("Walking Dependancies")]
    [SerializeField] WalkingController walkingController;

    [Header("Movement Dependancies")]
    [SerializeField] PlayerMovement playerMovement;

    [Header("Get Up Dependancies")]
    [SerializeField] GetUpByJumping getUp;

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
    public float stepDuration = 0.2f;
    public float stepHeight = 1.7f;
    public float feetMountForce = 25f;

    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;

    [HideInInspector] public float timer;
    [HideInInspector] public float stepTimerRight;
    [HideInInspector] public float stepTimerLeft;
    [HideInInspector] public float MouseYAxisArms;
    [HideInInspector] public float MouseYAxisBody;

    [HideInInspector] public bool walkForward;
    [HideInInspector] public bool walkBackward;
    [HideInInspector] public bool stepRight;
    [HideInInspector] public bool stepLeft;
    [HideInInspector] public bool alertLegRight;
    [HideInInspector] public bool alertLegLeft;
    [HideInInspector] public bool balanced = true;
    [HideInInspector] public bool isKeyDown;
    [HideInInspector] public bool moveAxisUsed;
    [HideInInspector] public bool jumpAxisUsed;
    [HideInInspector] public bool reachLeftAxisUsed;
    [HideInInspector] public bool reachRightAxisUsed;
    [HideInInspector] public bool jumping, isJumping;
    [HideInInspector] public bool inAir;
    [HideInInspector] public bool punchingRight;
    [HideInInspector] public bool punchingLeft;
    [HideInInspector] public bool isRagdoll;
    [HideInInspector] public bool resetPose;

    [HideInInspector] public Camera cam;

    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 centerOfMassPoint;

    //Player Parts Array
    [HideInInspector] public ConfigurableJoint[] playerParts;

    //JointDrives in Unity are used to control the motion of joints in a physics simulation.
    //They allow for greater control over the movement of joints by allowing you to set target rotation and velocity values, along with positional spring and damping values.
    //This can help create realistic and stable movements in your physics simulations.
    //Joint Drives on & off
    [HideInInspector] public JointDrive BalanceOn;
    [HideInInspector] public JointDrive PoseOn;
    [HideInInspector] public JointDrive CoreStiffness;
    [HideInInspector] public JointDrive ReachStiffness;
    [HideInInspector] public JointDrive DriveOff;

    //Original pose target rotation
    [HideInInspector] public Quaternion HeadTarget;
    [HideInInspector] public Quaternion BodyTarget;
    [HideInInspector] public Quaternion UpperRightArmTarget;
    [HideInInspector] public Quaternion LowerRightArmTarget;
    [HideInInspector] public Quaternion UpperLeftArmTarget;
    [HideInInspector] public Quaternion LowerLeftArmTarget;
    [HideInInspector] public Quaternion UpperRightLegTarget;
    [HideInInspector] public Quaternion LowerRightLegTarget;
    [HideInInspector] public Quaternion UpperLeftLegTarget;
    [HideInInspector] public Quaternion LowerLeftLegTarget;

    private void Awake()
    {
        playerSetup.SetupPlayer();
    }

    private void Update()
    {
        if (useControls && !inAir)
        {
            playerMovement.Movement();
        }

        if (useControls)
        {
            PlayerReach();
        }

        if (balanced && useStepPrediction)
        {
            stepPrediction.PredictNextStep();
        }

        if (!useStepPrediction)
        {
            resetWalkCycle.WalkCycleReset();
        }

        groundCheck.GroundChecker(playerParts[0], 
            balanceHeight, 
            inAir, 
            isJumping, 
            reachRightAxisUsed, 
            reachLeftAxisUsed, 
            ref balanced, 
            autoGetUpWhenPossible);

        ragdollCheck.RagdollChecker(balanced, 
            isRagdoll);

        centerOfMass.CenterOfMassCalculation(COMP, 
            centerOfMassPoint, 
            playerParts);
    }

    private void FixedUpdate()
    {
        walkingController.Walking();

        if (useControls)
        {
            playerRotation.PlayerRotationCalculation(cam, 
                playerParts[0], 
                turnSpeed);

            resetPlayerPose.PlayerPoseReset();

            getUp.PlayerGetUpJumping();
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
            playerParts[1].targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }

        //Handle player's left reach
        if (Input.GetAxisRaw(reachLeft) != 0)
        {
            if (!reachLeftAxisUsed)
            {
                //Adjust Left Arm joint strength
                playerParts[5].angularXDrive = ReachStiffness;
                playerParts[5].angularYZDrive = ReachStiffness;
                playerParts[6].angularXDrive = ReachStiffness;
                playerParts[6].angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                playerParts[1].angularXDrive = CoreStiffness;
                playerParts[1].angularYZDrive = CoreStiffness;

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
            playerParts[5].targetRotation = new Quaternion(-0.58f - (MouseYAxisArms), -0.88f - (MouseYAxisArms), -0.8f, 1);
        }

        if (Input.GetAxisRaw(reachLeft) == 0)
        {
            //Sets the left reach input is not being used (specified by the reachLeft variable), and if so,
            //it resets the stiffness of the ConfigurableJoint components attached to playerParts[5], playerParts[6], and playerParts[1]
            if (reachLeftAxisUsed)
            {
                if (balanced)
                {
                    playerParts[5].angularXDrive = PoseOn;
                    playerParts[5].angularYZDrive = PoseOn;
                    playerParts[6].angularXDrive = PoseOn;
                    playerParts[6].angularYZDrive = PoseOn;

                    playerParts[1].angularXDrive = PoseOn;
                    playerParts[1].angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    playerParts[5].angularXDrive = DriveOff;
                    playerParts[5].angularYZDrive = DriveOff;
                    playerParts[6].angularXDrive = DriveOff;
                    playerParts[6].angularYZDrive = DriveOff;
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
                playerParts[3].angularXDrive = ReachStiffness;
                playerParts[3].angularYZDrive = ReachStiffness;
                playerParts[4].angularXDrive = ReachStiffness;
                playerParts[4].angularYZDrive = ReachStiffness;

                //Adjust body joint strength
                playerParts[1].angularXDrive = CoreStiffness;
                playerParts[1].angularYZDrive = CoreStiffness;

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
                    playerParts[3].angularXDrive = PoseOn;
                    playerParts[3].angularYZDrive = PoseOn;
                    playerParts[4].angularXDrive = PoseOn;
                    playerParts[4].angularYZDrive = PoseOn;

                    playerParts[1].angularXDrive = PoseOn;
                    playerParts[1].angularYZDrive = PoseOn;
                }
                else if (!balanced)
                {
                    playerParts[3].angularXDrive = DriveOff;
                    playerParts[3].angularYZDrive = DriveOff;
                    playerParts[4].angularXDrive = DriveOff;
                    playerParts[4].angularYZDrive = DriveOff;
                }                 

                resetPose = true;
                reachRightAxisUsed = false;
            }
        }
    }
}