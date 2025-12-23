using UnityEngine;
using UnityEngine.UIElements;

public class RotateOnAxis : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Axis rotateOn;

    [Header("For constant rotation (for kinematic only)")]
    [SerializeField] private float rotationSpeed = 90f; // degrees per second
    
    [Header("For dynamic rotation (for non-kinematic only)")]
    [SerializeField] float maxAngle = 90f;
    [SerializeField] float torque = 50f;
    [SerializeField] int direction = 1;
    private bool canChangeDir = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.isKinematic) //constant rotation
        {
            Vector3 Euler = GetEulerOnAxis();
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationSpeed * Time.fixedDeltaTime, 0f));
        }
        else
        {
            float angle = GetSignedAngle();
            // Reverse direction at limits
            if (Mathf.Abs(angle) >= maxAngle && canChangeDir)
            {
                direction *= -1;
                canChangeDir = false;
            }
            if (Mathf.Abs(angle) < maxAngle)
            {
                canChangeDir = true;
            }
            
            rb.AddTorque(GetVectorOnAxis() * torque * direction);
            rb.maxAngularVelocity = 10f;

        }
        
    }

    Vector3 GetVectorOnAxis()
    {
        return rotateOn switch
        {
            Axis.X => transform.right,
            Axis.Y => transform.up,
            Axis.Z => transform.forward,
            _ => Vector3.zero
        };
    }

    Vector3 GetEulerOnAxis()
    {
        float angle = rotationSpeed * Time.fixedDeltaTime;
        return rotateOn switch
        {
            Axis.X => new Vector3(angle, 0f, 0f),
            Axis.Y => new Vector3(0f, angle, 0f),
            Axis.Z => new Vector3(0f, 0f, angle),
            _ => Vector3.zero
        };
    }

    float GetSignedAngle()
    {
        float angle = rotateOn switch
        {
            Axis.X => transform.localEulerAngles.x,
            Axis.Y => transform.localEulerAngles.y,
            Axis.Z => transform.localEulerAngles.z,
            _ => 0f
        }; 
        return angle > 180f ? angle - 360f : angle;
    }

    enum Axis
    {
        X, Y, Z
    }
}