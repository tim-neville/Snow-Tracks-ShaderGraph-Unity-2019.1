using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DriveType
{
    Front,
    Rear,
    AllWheelDrive
}

public class VehicleController : MonoBehaviour
{
    public DriveType driveType = DriveType.AllWheelDrive;

    public WheelCollider wheelFrontLeft;
    public WheelCollider wheelFrontRight;

    public WheelCollider driveWheelRearLeft;
    public WheelCollider driveWheelRearRight;

    public Vector3 centerOfMass = new Vector3(0f, -0.35f, 0f);
    public bool showCenterOfMass = false;
    private Rigidbody _rigidBody;

    public float maxSteeringAngle = 30f;

    public float maxMotorTorque = 700f;
    public float currentSpeed = 0f;
    public float maxSpeed = 100f;

    public float maxBrakeTorque = 2000f;
    private bool isBraking = false;

    public MeshRenderer brakeLightLeft;
    public MeshRenderer brakeLightRight;
    public MeshRenderer reverseLightLeft;
    public MeshRenderer reverseLightRight;

    [ColorUsageAttribute(true, true)]
    public Color redBrakeLightColor;

    [ColorUsageAttribute(true, true)]
    public Color whiteReverseLightColor;

    float leftStickInput = 0f;
    float leftTriggerInput = 0f;
    float rightTriggerInput = 0f;
    bool yButton = false;
    bool aButton = false;

    WheelFrictionCurve originalRearFrictionCurve;
    WheelFrictionCurve OnHandBrakeRearFrictionCurve;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidBody.centerOfMass = centerOfMass;
        originalRearFrictionCurve = driveWheelRearLeft.sidewaysFriction;
        OnHandBrakeRearFrictionCurve = driveWheelRearLeft.sidewaysFriction;
        OnHandBrakeRearFrictionCurve.stiffness = 0.2f;
    }

    void Update()
    {
        leftStickInput = Input.GetAxisRaw("LeftStickJoystick1");
        leftTriggerInput = Input.GetAxisRaw("LeftTriggerJoystick1");
        rightTriggerInput = Input.GetAxisRaw("RightTriggerJoystick1");
        yButton = Input.GetButton("YButtonJoystick1");
        aButton = Input.GetButton("AButtonJoystick1");
    }

    void FixedUpdate()
    {
        ApplySteer();
        Accelerate();
        Braking();
    }

    private void ApplySteer()
    {
        float steeringAmount = leftStickInput * maxSteeringAngle;

        if (Mathf.Abs(leftStickInput) > 0.1f)
        {
            wheelFrontLeft.steerAngle = steeringAmount;
            wheelFrontRight.steerAngle = steeringAmount;
        }
        else
        {
            wheelFrontLeft.steerAngle = 0f;
            wheelFrontRight.steerAngle = 0f;
        }
    }

    private void Accelerate()
    {
        currentSpeed = 2 * Mathf.PI * wheelFrontLeft.radius * wheelFrontLeft.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            if (rightTriggerInput > 0.1f)
            {
                ApplyDriveToWheels(rightTriggerInput * maxMotorTorque);
                ShowReverseLights(false);
            }
            else
            if (yButton)
            {
                ApplyDriveToWheels(-maxMotorTorque);
                ShowReverseLights(true);
            }
            else
            {
                ApplyDriveToWheels(0);
                ShowReverseLights(false);
            }
        }
        else
        {
            ApplyDriveToWheels(0);
        }

    }

    private void Braking()
    {
        if (leftTriggerInput > 0.1f)
        {
            brakeLightLeft.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 3);
            brakeLightRight.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 3);
            ApplyBrakesToWheels(leftTriggerInput * maxBrakeTorque);
        }
        else
        if (!aButton)
        {
            brakeLightLeft.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 0);
            brakeLightRight.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 0);
            ApplyBrakesToWheels(0);
        }
        if (aButton)
        {
            brakeLightLeft.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 3);
            brakeLightRight.material.SetColor("_EmissionColor", new Vector4(redBrakeLightColor.r, redBrakeLightColor.g, redBrakeLightColor.b, 0) * 3);
            ApplyBrakesToRearWheels();
        }
        else
        {
            driveWheelRearLeft.sidewaysFriction = originalRearFrictionCurve;
            driveWheelRearRight.sidewaysFriction = originalRearFrictionCurve;
        }
    }

    private void ApplyDriveToWheels(float torque)
    {
        switch (driveType)
        {
            case DriveType.Front:
                wheelFrontLeft.motorTorque = torque * 0.5f;
                wheelFrontRight.motorTorque = torque * 0.5f;
                break;
            case DriveType.Rear:
                driveWheelRearLeft.motorTorque = torque * 0.5f;
                driveWheelRearRight.motorTorque = torque * 0.5f;
                break;
            case DriveType.AllWheelDrive:
                wheelFrontLeft.motorTorque = torque * 0.4f * 0.5f;
                wheelFrontRight.motorTorque = torque * 0.4f * 0.5f;
                driveWheelRearLeft.motorTorque = torque * 0.6f * 0.5f;
                driveWheelRearRight.motorTorque = torque * 0.6f * 0.5f;
                break;
        }
    }

    private void ApplyBrakesToWheels(float brakeTorque)
    {
        wheelFrontLeft.brakeTorque = brakeTorque * 0.6f * 0.5f;
        wheelFrontRight.brakeTorque = brakeTorque * 0.6f * 0.5f;
        driveWheelRearLeft.brakeTorque = brakeTorque * 0.4f * 0.5f;
        driveWheelRearRight.brakeTorque = brakeTorque * 0.4f * 0.5f;
    }

    private void ApplyBrakesToRearWheels()
    {
        driveWheelRearLeft.brakeTorque = maxBrakeTorque;
        driveWheelRearRight.brakeTorque = maxBrakeTorque;

        driveWheelRearLeft.sidewaysFriction = OnHandBrakeRearFrictionCurve;
        driveWheelRearRight.sidewaysFriction = OnHandBrakeRearFrictionCurve;
    }

    private void ShowReverseLights(bool state)
    {
        if (state)
        {
            reverseLightLeft.material.SetColor("_EmissionColor", new Vector4(whiteReverseLightColor.r, whiteReverseLightColor.g, whiteReverseLightColor.b, 0) * 3);
            reverseLightRight.material.SetColor("_EmissionColor", new Vector4(whiteReverseLightColor.r, whiteReverseLightColor.g, whiteReverseLightColor.b, 0) * 3);
        }
        else
        {
            reverseLightLeft.material.SetColor("_EmissionColor", new Vector4(whiteReverseLightColor.r, whiteReverseLightColor.g, whiteReverseLightColor.b, 0) * 0);
            reverseLightRight.material.SetColor("_EmissionColor", new Vector4(whiteReverseLightColor.r, whiteReverseLightColor.g, whiteReverseLightColor.b, 0) * 0);
        }

    }

    private void OnDrawGizmos()
    {
        if (showCenterOfMass)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(centerOfMass), 0.1f);
        }
    }
}
