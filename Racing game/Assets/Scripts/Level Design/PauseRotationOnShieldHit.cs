using UnityEngine;

public class PauseRotationOnShieldHit : MonoBehaviour
{
    RotateOnAxis rotator;
    GameObject[] triggers;
    float pauseTime = 2;
    float timer = 0;
    bool stopPause = false;
    [SerializeField] VoidEventChannelSO shieldEnded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotator = GetComponent<RotateOnAxis>();
        if (rotator == null)
            Debug.LogError("No RotateOnAxis script was found", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopPause) return;
        if (timer + pauseTime < Time.time)
        {
            UnpauseMovement();
            stopPause = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            PauseMovement();
            shieldEnded.OnEventRaised += UnpauseMovementAfterTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            UnpauseMovementAfterTime();
            shieldEnded.OnEventRaised -= UnpauseMovementAfterTime;
        }
    }

    public void UnpauseMovementAfterTime()
    {
        stopPause = true;
        timer = Time.time;
    }

    public void PauseMovement()
    {
        rotator.PauseMovement();
        stopPause = false;
    }

    public void UnpauseMovement()
    {
        rotator.UnpauseMovement();
    }

}
