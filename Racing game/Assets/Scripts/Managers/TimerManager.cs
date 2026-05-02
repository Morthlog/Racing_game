using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    Timer timer;

    public static TimerManager instance;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO timerStarted;
    [SerializeField] private VoidEventChannelSO timerStopped;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        timerStarted.RaiseEvent();
        timer.StartTimer();
    }

    public void StopTimer()
    {
        timerStopped.RaiseEvent();
        timer.StopTimer();
    }

    public void OnLap()
    {
        timer.HighlightTimer();
    }

    public float GetTime()
    {
        return timer.GetTime();
    }
}
