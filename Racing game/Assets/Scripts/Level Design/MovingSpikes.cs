using System.Collections.Generic;
using UnityEngine;

// Move the spikes forwards and backwards in local space to simulate opening and closing
public class MovingSpikes : MonoBehaviour, TriggerController
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

    private Dictionary<GameObject, int> triggerCount;
    private int maxTriggers = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closePos = Vector3.zero;

        foreach (Transform transform in gameObject.transform)
        {
            if (transform.CompareTag("Spikes"))
            {
                spikes = transform.gameObject;
                break;
            }
        }
        openPos = spikes.transform.localPosition;
        rb = spikes.GetComponent<Rigidbody>();

        triggerCount = new Dictionary<GameObject, int>();
        timer = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = Vector3.zero;
        switch (state)
        {
            case RunState.Opening:
                movement = -Vector3.forward;

                if (spikes.transform.localPosition.z >= openPos.z)
                    break;
                TransitionTo(RunState.Opened);
                break;

            case RunState.Closing:
                movement = Vector3.forward;

                if (spikes.transform.localPosition.z <= closePos.z)
                    break;
                TransitionTo(RunState.Closed);
                break;

            case RunState.Closed:
                TryWake(RunState.Opening, -Vector3.forward);
                break;

            case RunState.Opened:
                TryWake(RunState.Closing, Vector3.forward);
                break;
        }
        Vector3 worldMovement = spikes.transform.TransformDirection(movement);
        rb.MovePosition(rb.position + worldMovement * maxSpeed * Time.fixedDeltaTime);

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

    public void OnObjectEnter(GameObject go)
    {
        var damageable = go.GetComponentInParent<IDamageable>();
        if (triggerCount.ContainsKey(go))
            triggerCount[go]++;
        else
            triggerCount[go] = 1;

        if (triggerCount[go] < maxTriggers)
            return;
        if (damageable == null) return;

        damageable.GetSquished();
    }

    public void OnObjectExit(GameObject go)
    {
        triggerCount[go]--;
    }

    enum RunState
    {
        Opening,
        Closing,
        Closed,
        Opened
    }
}
