using System.Collections.Generic;
using System.Linq;
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
    private GameObject spikesTrigger;
    private Rigidbody rb;
    private float timer;
    [SerializeField] private float sleepTime = 1f; // in sec
    [SerializeField] private float pauseOnShieldTime = 2f; // in sec
    private bool canMove = true;

    private Dictionary<string, Dictionary<GameObject, int>> triggerCount;
    private int maxTriggers = 2;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO shieldEnded;
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

        foreach (Transform transform in spikes.transform)
        {
            if (transform.CompareTag("Trigger"))
            {
                spikesTrigger = transform.gameObject;
                break;
            }
        }
        openPos = spikes.transform.localPosition;
        rb = spikes.GetComponent<Rigidbody>();

        triggerCount = new Dictionary<string, Dictionary<GameObject, int>>();
        timer = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;
        if (!TryWake(state, movement))
            return;
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
        timer = Time.time + sleepTime;
        movement = Vector3.zero;
    }
    bool TryWake(RunState nextState, Vector3 dir)
    {
        if (Time.time <= timer)
            return false;

        state = nextState;
        movement = dir;
        return true;
    }

    public void OnObjectEnter(GameObject origin, GameObject go)
    {
        var damageable = go.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        string parent = GameLoopManager.instance.GetParentTag(go);
        if (!triggerCount.ContainsKey(parent))
            triggerCount[parent] = new Dictionary<GameObject, int>();

        if (origin == spikesTrigger && go.CompareTag("Shield"))
        {
            PauseMovement();
            return;
        }

        Dictionary<GameObject, int> inner = triggerCount[parent];

        if (inner.ContainsKey(go))
            inner[go]++;
        else
            inner[go] = 1;

        if (inner[go] < maxTriggers)
            return;

        damageable.GetSquished();

        var keys = inner.Keys.ToList();
        foreach (var key in keys)
        {
            inner[key] = 0;
        }
    }

    public void OnObjectExit(GameObject origin, GameObject go) // not called if object is disabled
    {
        if (origin == spikesTrigger && go.CompareTag("Shield"))
        {
            EnableMovementAfterTime();
            return;
        }
        string parent = GameLoopManager.instance.GetParentTag(go);

        triggerCount[parent][go]--;
    }

    enum RunState
    {
        Opening,
        Closing,
        Closed,
        Opened
    }

    private void PauseMovement()
    {
        canMove = false;
        shieldEnded.OnEventRaised += EnableMovementAfterTime;
    }

    public void EnableMovementAfterTime()
    {
        shieldEnded.OnEventRaised -= EnableMovementAfterTime;
        if (canMove)
            return;
        timer = Time.time + pauseOnShieldTime;
        canMove = true;
    }

    private void OnDisable()
    {
        shieldEnded.OnEventRaised -= EnableMovementAfterTime;
    }
}
