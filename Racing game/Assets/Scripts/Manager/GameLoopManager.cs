using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;
    private float time;
    private int frames;
    private int playerLastCheckpointID = 0;
    private GameObject player;
    private Vector3 playerSpawnPoint;
    private Dictionary<int, Checkpoint> checkpoints;

    private float rotationDiffCheckpointPlayer = 90f;

    private bool iPaused = false;
    private bool allowMovement = false;

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
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawnPoint = player.transform.position;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoints = new Dictionary<int, Checkpoint>(gos.Length);
        for (int i = 0; i < gos.Length; i++)
        {
            Checkpoint ch = gos[i].GetComponentInChildren<Checkpoint>();
            checkpoints[ch.ID] = ch;
        }

        AddDefaultCheckpoint();
    
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
        int nextID = checkpoints[playerLastCheckpointID].NextID();
        if (nextID == checkpointID)
        {
            Debug.Log("Hit correct checkpoint");
            playerLastCheckpointID = checkpointID;
            playerSpawnPoint = checkpoints[checkpointID].GetComponent<Transform>().position;
        }
        else
        {
            Debug.Log("Checkpoint was hit too soon");
        }
    }

    public void toLastCheckpoint()
    {
        Transform tr = player.transform;
        tr.position = playerSpawnPoint;
        Vector3 r = checkpoints[playerLastCheckpointID].transform.parent.transform.localEulerAngles;
        r.y += rotationDiffCheckpointPlayer;
        tr.localEulerAngles = r;

        player.GetComponentInChildren<CarControl>().MovementReset();
    }

    public string getParentTag(GameObject obj)
    {
        Transform current = obj.transform;
        Transform prev = null;
        while (current != null)
        {
            prev = current;
            current = current.parent;
        }

        return prev.tag;
    }

    public bool IPaused()
    {
        return iPaused;
    }

    public void SetPaused(bool paused)
    { 
        iPaused = paused; 
    }

    public bool AllowMovement()
    {
        return allowMovement;
    }

    public void StartRound()
    {
        SetAllowMovement(true);
        StartTimer();
    }
    public void SetAllowMovement(bool allowMovement)
    {
        this.allowMovement = allowMovement;
    }

    private void StartTimer()
    {

    }

    private void AddDefaultCheckpoint()
    {
        GameObject checkpointTrigger = new GameObject("Trigger 0");

        Checkpoint checkpoint = checkpointTrigger.AddComponent<Checkpoint>();
        checkpointTrigger.transform.SetParent(new GameObject("Checkpoint Default").transform);
        Vector3 r = player.transform.eulerAngles;
        r.y -= rotationDiffCheckpointPlayer;
        checkpointTrigger.transform.parent.transform.eulerAngles = r;
        checkpoint.SetID(0);
        checkpoint.SetNextID(1);
        checkpoints[0] = checkpoint;
    }
}
