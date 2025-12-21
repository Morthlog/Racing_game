using System;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;
    private float time;
    private int frames;
    private int playerLastCheckpoint = 0;

    private void Awake()
    {
        Debug.Log("Awake");
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = Time.time;
        frames = 0;
    }

    // Update is called once per frames
    void Update()
    {
        //fpsCounter();
    }

    private void fpsCounter()
    {
        frames++;
        if (Time.time - time > 1)
        {
            Debug.Log("FPS: " + frames);
            time = Time.time;
            frames = 0;
        }
    }

    public void OnCheckpointHit(GameObject go, int checkpointID)
    {
        Debug.Log($"Triggered by {go.name} on {checkpointID}");
        if (checkpointID - playerLastCheckpoint == 1)
        {
            Debug.Log("Hit correct checkpoint");
            playerLastCheckpoint++;
        }
        else
        {
            Debug.Log("Checkpoint was hit too soon");
        }
    }
}
