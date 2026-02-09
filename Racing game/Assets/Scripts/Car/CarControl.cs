using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CarControl : MonoBehaviour
{
    [Header("Car Properties")]
    public float defaultMotorTorque = 2000f;
    public float boostMotorTorque = 4000f;
    public float brakeTorque = 2000f;
    [SerializeField] float boostTargetSpeed = 25f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;

    private GameInputActions actions; // Reference to the new input system


    [SerializeField] Light[] brakeLights;
    [SerializeField] bool addBoost;
    [SerializeField] float slowDownForce = 0.3f;

    [Header("Events")]
    [SerializeField] private IntEventChannelSO speedBoostUsed;

    void Awake()
    {
        actions = new GameInputActions(); // Initialize Input Actions
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass to improve stability and prevent rolling
        Vector3 centerOfMass = rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfMass;

        // Get all wheel components attached to the car
        wheels = GetComponentsInChildren<WheelControl>();
    }

    void OnEnable()
    {
        actions.Car.Enable();
        speedBoostUsed.OnEventRaised += EnableSpeedBoost;
    }

    void OnDisable()
    {
        actions.Car.Disable();
        speedBoostUsed.OnEventRaised -= EnableSpeedBoost;
    }


    IEnumerator DisableSpeedBoost(int duration)
    {
        yield return new WaitForSeconds(duration);
        addBoost = false;
    }

    void EnableSpeedBoost(int duration)
    {
        addBoost= true;
        StartCoroutine(DisableSpeedBoost(duration));
    }


    // FixedUpdate is called at a fixed time interval
    void FixedUpdate()
    {
        float motorTorque = defaultMotorTorque;

        if (addBoost)
        {
            //motorTorque = boostMotorTorque;
            ApplyBoostHardSet();
        }

        // Read the Vector2 input from the new Input System
        Vector2 inputVector = actions.Car.Movement.ReadValue<Vector2>();

        // Get player input for acceleration and steering
        float vInput = inputVector.y; // Forward/backward input
        float hInput = inputVector.x; // Steering input

        // Calculate current speed along the car's forward axis
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); // Normalized speed factor

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        bool isTryingToMoveSameDirection = (vInput > 0 && forwardSpeed > -0.1f) || (vInput < 0 && forwardSpeed < 0.1f);

        bool brakes = (vInput < 0 && forwardSpeed > 0.1f) || (vInput > 0 && forwardSpeed < -0.1f);
        SetLights(brakes);
        

        foreach (var wheel in wheels)
        {
            // Apply steering to wheels that support steering
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isTryingToMoveSameDirection)
            {
                // Apply torque to motorized wheels
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                // Release brakes when accelerating
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                // Apply brakes when reversing direction
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
            }
        }

        SlowDownCar(inputVector);

    }
    void ApplyBoostHardSet()
    {
        Vector3 v = rigidBody.linearVelocity;
        Vector3 forward = transform.forward;

        float currentForward = Vector3.Dot(v, forward);
        float target = boostTargetSpeed;

        if (currentForward >= target) return;

        // Διατηρεί πλευρική/κάθετη συνιστώσα, αντικαθιστά μόνο τη forward
        Vector3 lateral = v - forward * currentForward;
        rigidBody.linearVelocity = lateral + forward * target;
    }

    void SlowDownCar( Vector2 inputVector)
    {
        if (inputVector == Vector2.zero)
        {
            rigidBody.linearDamping = slowDownForce;
        }
        else
        {
            rigidBody.linearDamping = 0;
        }
    }

    void SetLights(bool brakes)
    {
        foreach (var light in brakeLights)
        {
            light.enabled = brakes;
        }
    }

    public void MovementReset()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.rotationSpeed = 0f;
        }
    }
}
