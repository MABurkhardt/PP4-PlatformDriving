using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class APCCarController : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private Rigidbody rb;
    private bool upButtonIsReady = true;
    private Quaternion initialRotation;
    //private PlayerControls controls;
    //[SerializeField] private GameObject go;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;

    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform rearLeftTransform;
    [SerializeField] private Transform rearRightTransform;

    [SerializeField] private TextMeshProUGUI carSpeed;

    private void Awake()
    {
        initialRotation = body.transform.rotation;
        //controls = new PlayerControls();
    }

    private void Start()
    {
        StewartPlatformController.singleton.Init("COM7", 115200);
    }

    //private void OnEnable()
    //{
    //    controls.Gameplay.Enable();
    //}

    //private void OnDisable()
    //{
    //    controls.Gameplay.Disable();
    //}

    private void Update()
    {
        GetInput();
        ShowSpeed();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        //steering
        horizontalInput = Input.GetAxis(HORIZONTAL);

        //forward/reverse/light brake
        verticalInput = Input.GetAxis("RightTrigger");
        if (Input.GetAxis("LeftTrigger") > 0)
        {
            verticalInput = -Input.GetAxis("LeftTrigger");
            ReverseMotor();
        }

        //handbrake
        if (Input.GetAxis("AButton") == 1)
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }

        //reset player
        if (Input.GetAxis("DPadUp") > 0)
        {
            if (upButtonIsReady)
            {
                upButtonIsReady = false;
                
                //teleport player up 2 units to allow space to rotate
                body.transform.position += new Vector3(0, 2, 0);

                //rotate car facing same direction but angled upright
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Vector3 v = body.transform.localEulerAngles;
                v.x = 0;
                v.z = 0;
                body.transform.localEulerAngles = v;
            }
        }
        else
        {
            upButtonIsReady = true;
        }
    }

    private void HandleMotor()
    {
        currentBreakForce = isBreaking ? breakForce : 0f;

        frontLeftCollider.motorTorque = verticalInput * motorForce;
        frontRightCollider.motorTorque = verticalInput * motorForce;
        rearLeftCollider.motorTorque = verticalInput * motorForce;
        rearRightCollider.motorTorque = verticalInput * motorForce;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeftCollider.brakeTorque = currentBreakForce;
        frontRightCollider.brakeTorque = currentBreakForce;
        rearLeftCollider.brakeTorque = currentBreakForce;
        rearRightCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftCollider.steerAngle = currentSteerAngle;
        frontRightCollider.steerAngle = currentSteerAngle;
        rearLeftCollider.steerAngle = -currentSteerAngle * .5f;
        rearRightCollider.steerAngle = -currentSteerAngle * .5f;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftCollider, frontLeftTransform);
        UpdateSingleWheel(frontRightCollider, frontRightTransform);
        UpdateSingleWheel(rearLeftCollider, rearLeftTransform);
        UpdateSingleWheel(rearRightCollider, rearRightTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void ReverseMotor()
    {
        currentBreakForce = isBreaking ? breakForce : 0f;

        frontLeftCollider.motorTorque = -verticalInput * motorForce;
        frontRightCollider.motorTorque = -verticalInput * motorForce;
        rearLeftCollider.motorTorque = -verticalInput * motorForce;
        rearRightCollider.motorTorque = -verticalInput * motorForce;

        ApplyBreaking();
    }

    private void ShowSpeed()
    {
        int roundedSpeed = (int)rb.velocity.magnitude;
        carSpeed.text =  roundedSpeed + " mph";
    }
}