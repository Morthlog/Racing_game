using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    Timer timer;

    public static TimerManager instance;
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
        timer.StartTimer();
    }

    public void StopTimer()
    {
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
