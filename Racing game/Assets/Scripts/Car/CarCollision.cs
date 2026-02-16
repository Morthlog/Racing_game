using UnityEngine;

public class CarCollision : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO outOfCameraLimits;
    [SerializeField] VoidEventChannelSO playerRespawned;
    private bool isOutOfBounds = false;

    void OnTriggerEnter(Collider other)
    {

        if (isOutOfBounds) return;

        if (other.CompareTag("CameraBound"))
        {
            isOutOfBounds = true; 
            outOfCameraLimits.RaiseEvent();
        }
    }

    public void ResetOutOfBounds()
    {
        isOutOfBounds = false;
    }

    private void OnEnable()
    {
        playerRespawned.OnEventRaised += ResetOutOfBounds;
    }


    private void OnDisable()
    {
        playerRespawned.OnEventRaised -= ResetOutOfBounds;

    }
}
