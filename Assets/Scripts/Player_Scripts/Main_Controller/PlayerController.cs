﻿using UnityEngine;

/// <summary>
/// Controls the movement and actions of the player character using ragdoll physics and joint drives.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Active Ragdoll Player parts")]
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

    [Header("Ground Dependencies")]
    [SerializeField] private CheckerGround groundCheck;

    [Header("Ragdoll Activator Dependencies")]
    [SerializeField] private ActivateRagdoll activateRagdoll;

    [Header("Ragdoll Deactivator Dependencies")]
    [SerializeField] private DeactivateRagdoll deactivateRagdoll;

    [Header("Center of Mass Dependencies")]
    [SerializeField] private CenterOfMass centerOfMass;

    [Header("Rotation Dependencies")]
    [SerializeField] private PlayerRotation playerRotation;

    [Header("Reset Player's Pose Dependencies")]
    [SerializeField] private ResetPlayerPose resetPlayerPose;

    [Header("Reset Walk Cycle Dependencies")]
    [SerializeField] private ResetWalkCycle resetWalkCycle;

    [Header("Ragdoll Check Dependencies")]
    [SerializeField] private RagdollCheck ragdollCheck;

    [Header("Player Setup Dependencies")]
    [SerializeField] private PlayerSetup playerSetup;

    [Header("Step Prediction Dependencies")]
    [SerializeField] private StepPrediction stepPrediction;

    [Header("Walking Dependencies")]
    [SerializeField] private WalkingController walkingController;

    [Header("Movement Dependencies")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Get Up Dependencies")]
    [SerializeField] private GetUpByJumping getUp;

    [Header("Hands Reach Dependencies")]
    [SerializeField] private PlayerReach handsReach;

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

    /// <summary>
    /// Sets up the player character.
    /// </summary>
    private void Awake()
    {
        playerSetup.SetupPlayer();
    }

    /// <summary>
    /// Updates the player's hand reach, performs step prediction (if enabled), checks if the player is on the ground or in the air, and checks for ragdoll state.
    /// </summary>
    private void Update()
    {
        handsReach.ReachHands();

        if (balanced && useStepPrediction)
        {
            stepPrediction.PredictNextStep();
        }

        if (!useStepPrediction)
        {
            resetWalkCycle.WalkCycleReset();
        }

        groundCheck.GroundChecker();

        ragdollCheck.RagdollChecker(balanced,
            isRagdoll);

        centerOfMass.CenterOfMassCalculation(COMP,
            centerOfMassPoint,
            playerParts);
    }

    /// <summary>
    /// Performs walking motion, calculates player rotation, resets the player's pose, and handles player get-up actions.
    /// </summary>
    private void FixedUpdate()
    {
        walkingController.Walking();

        playerRotation.PlayerRotationCalculation(cam,
            playerParts[0],
            turnSpeed);

        resetPlayerPose.PlayerPoseReset();

        getUp.PlayerGetUpJumping();
    }
}