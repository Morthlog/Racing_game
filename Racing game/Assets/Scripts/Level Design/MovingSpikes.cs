using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    private Vector3 openPos;
    private Vector3 closePos;
    [SerializeField] private RunState state;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float accelaration = 5f;
    private Vector3 movement;
    private GameObject spikes;
    private Rigidbody rb;
    private float timer;
    [SerializeField] private float sleepTime = 1f; // in sec
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closePos = gameObject.transform.position;
        
        foreach (Transform transform in gameObject.transform)
        {
            if (transform.CompareTag("Spikes"))
            {
                spikes = transform.gameObject;
                break;
            }
        }
        openPos = spikes.transform.position;
        rb = spikes.GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        if (state == RunState.Opening)
            spikes.transform.position = openPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = Vector3.zero;
        switch (state)
        {
            case RunState.Opening:
                movement = -spikes.transform.forward;

                if (spikes.transform.position.x >= openPos.x)
                    break;
                TransitionTo(RunState.Opened);
                break;

            case RunState.Closing:
                movement = spikes.transform.forward;

                if (spikes.transform.position.x <= closePos.x)
                    break;
                TransitionTo(RunState.Closed);
                break;

            case RunState.Closed:
                TryWake(RunState.Opening, -spikes.transform.forward);
                break;

            case RunState.Opened:
                TryWake(RunState.Closing, spikes.transform.forward);
                break;
        }

        rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);

    }
    void TransitionTo(RunState newState)
    {
        state = newState;
        timer = Time.time;
        movement = Vector3.zero;
    }
    void TryWake(RunState nextState, Vector3 dir)
    {
        if (Time.time - timer <= sleepTime)
            return;

        state = nextState;
        movement = dir;
    }

    enum RunState
    {
        Opening,
        Closing,
        Closed,
        Opened
    }
}
