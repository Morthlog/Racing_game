using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    private Vector3 openPos;
    private Vector3 closePos;
    [SerializeField] private runState state;
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
        if (state == runState.opening)
            spikes.transform.position = openPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = Vector3.zero;
        switch (state)
        {
            case runState.opening:
                movement = -spikes.transform.forward;

                if (spikes.transform.position.x >= openPos.x)
                    break;
                TransitionTo(runState.opened);
                break;

            case runState.closing:
                movement = spikes.transform.forward;

                if (spikes.transform.position.x <= closePos.x)
                    break;
                TransitionTo(runState.closed);
                break;

            case runState.closed:
                TryWake(runState.opening, -spikes.transform.forward);
                break;

            case runState.opened:
                TryWake(runState.closing, spikes.transform.forward);
                break;
        }

        rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);

    }
    void TransitionTo(runState newState)
    {
        state = newState;
        timer = Time.time;
        movement = Vector3.zero;
    }
    void TryWake(runState nextState, Vector3 dir)
    {
        if (Time.time - timer <= sleepTime)
            return;

        state = nextState;
        movement = dir;
    }

    enum runState
    {
        opening,
        closing,
        closed,
        opened
    }
}
