using UnityEngine;

public class CarCollision : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO enteredCameraLimit;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraBound"))
        {
            enteredCameraLimit.RaiseEvent();
        }
    }
}
