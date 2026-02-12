using UnityEngine;
using UnityEngine.UIElements;

public class RotateOnAxis : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private RotationType rotationType;
    [SerializeField] private Axis rotateOn;
    [SerializeField] Direction direction = Direction.Positive;

    [Header("For constant rotation (Kinematic only)")]
    [SerializeField] private float rotationSpeed = 90f; // degrees per second

    [Header("For constant rotation (Non-Kinematic only")]
    [SerializeField] private float rotationalSpeed = 5f;

    [Header("For dynamic rotation (Non-Kinematic only)")]
    [SerializeField] float maxAngle = 90f;
    [SerializeField] float torque = 50f;
    private bool canChangeDir = false;

    private bool canRotate = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!canRotate)
            return;
        switch (rotationType)
        {
            case RotationType.ConstantKinematic:
                Vector3 Euler = GetEulerOnAxis();
                rb.MoveRotation(rb.rotation * Quaternion.Euler(Euler * (rotationSpeed * (int)direction * Time.fixedDeltaTime)));
                break;

            case RotationType.ConstantNonKinematic:
                rb.angularVelocity = GetVectorOnAxis() * (rotationalSpeed * (int)direction);
                break;

            case RotationType.DynamicNonKinematic:
                float angle = GetSignedAngle();
                FindDirection(angle);

                rb.AddTorque(GetVectorOnAxis() * torque * (int)direction);
                rb.maxAngularVelocity = 10f;
                break;
        }
        
    }

    public void SetCanRotate(bool canRotate)
    {
        this.canRotate = canRotate;
    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
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

    public float GetSignedAngle()
    {
        float angle = GetAngle();
        return angle > 180f ? angle - 360f : angle;
    }

    public float GetAngle()
    {
        return rotateOn switch
        {
            Axis.X => transform.localEulerAngles.x,
            Axis.Y => transform.localEulerAngles.y,
            Axis.Z => transform.localEulerAngles.z,
            _ => 0f
        };
    }

    void FindDirection(float angle)
    {
        if (Mathf.Abs(angle) >= maxAngle && canChangeDir)
        {
            direction = direction == Direction.Positive
                    ? Direction.Negative
                    : Direction.Positive;
            canChangeDir = false;
        }
        if (Mathf.Abs(angle) < maxAngle)
        {
            canChangeDir = true;
        }
    }

    public void PauseMovement()
    {
        SetCanRotate(false);
        if (!(rotationType == RotationType.ConstantKinematic))
        {
            rb.isKinematic = true;
        }
            
    }
    
    public void UnpauseMovement()
    {
        SetCanRotate(true);
        if (!(rotationType == RotationType.ConstantKinematic))
        {
            rb.isKinematic = false;
        }
    }

    enum Axis
    {
        X, Y, Z
    }

    public enum Direction
    {
        Positive = 1,
        Negative = -1
    }

    enum RotationType
    {
        ConstantKinematic,
        ConstantNonKinematic,
        DynamicNonKinematic
    }
}