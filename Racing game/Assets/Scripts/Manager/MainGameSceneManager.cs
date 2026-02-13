using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneManager : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int initialLives = 3;
    [SerializeField] private int currentLives;

    public static MainGameSceneManager instance;
    private float time;
    private int frames;
    private int playerLastCheckpointID = 0;
    private GameObject player;
    private Vector3 playerSpawnPoint;
    private Dictionary<int, Checkpoint> checkpoints;

    private float rotationDiffCheckpointPlayer = 90f; // relative difference between Checkpoint and Player rotation in WC

    private bool iPaused = false;
    private bool allowMovement = false;
    [SerializeField] GameObject startPoint;
    [Header("Laps")]
    [SerializeField] private int totalLoops = 3;
    [SerializeField] private int currentLoop = 1;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO playerRespawned;
    [SerializeField] VoidEventChannelSO lapCompleted;
    [SerializeField] VoidEventChannelSO gameover;

    private bool hasHitFirstCheckpoint = false;
    bool isGameover = false;
    

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
        playerSpawnPoint =startPoint.transform.position;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoints = new Dictionary<int, Checkpoint>(gos.Length);
        for (int i = 0; i < gos.Length; i++)
        {
            Checkpoint ch = gos[i].GetComponentInChildren<Checkpoint>();
            if (checkpoints.TryGetValue(ch.ID, out Checkpoint duplicate))
            {
                Debug.LogError(
                    $"Duplicate Checkpoint ID {ch.ID} (ORIGINAL)", duplicate.gameObject);

                Debug.LogError(
                    $"Duplicate Checkpoint ID {ch.ID} (CURRENT)", ch.gameObject);
            }
                
            checkpoints[ch.ID] = ch;
        }

        AddDefaultCheckpoint();
    
        time = Time.time;
        frames = 0;

        ResetLives();
    }

    // Update is called once per frames
    void Update()
    {
        //FpsCounter();
    }

    private void FpsCounter()
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
        int[] nextIDs = checkpoints[playerLastCheckpointID].NextIDs();
        bool correctCheckpoint = false;
        foreach (int id in nextIDs)
        {
            if (id == checkpointID)
            {
                Debug.Log("Hit correct checkpoint");
                playerLastCheckpointID = checkpointID;
                playerSpawnPoint = checkpoints[checkpointID].GetComponent<Transform>().position;
                correctCheckpoint = true;

                if (checkpoints[checkpointID].IsStart())
                    UpdateCurrentLoop();

                break;
            }
        }
        if (!correctCheckpoint)
        {
            Debug.Log("Checkpoint was hit too soon");
        }
    }

    private void UpdateCurrentLoop()
    {
        String output = "Current loop ";
        if (hasHitFirstCheckpoint) // Has officially hit the start checkpoint
        {   
            currentLoop++;
            lapCompleted.RaiseEvent();
        }
        else
        {
            hasHitFirstCheckpoint = true;
        }
            
        output += currentLoop;
        if (currentLoop > totalLoops)
        {
            FinishRace();
            return;
        }
        Debug.Log(output);
    }

    private void FinishRace()
    {
        Debug.Log("Race is over");
        TimerManager.instance.StopTimer();
        GameManager.instance.AddTime(TimerManager.instance.GetTime());


        SceneManager.LoadSceneAsync("Celebration");
    }
    public void ToLastCheckpoint()
    {
        TransferToCheckpoint(playerLastCheckpointID, playerSpawnPoint);
    }

    private void TransferToCheckpoint(int id, Vector3 spawnpoint)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.MovePosition(spawnpoint);
        Vector3 r = checkpoints[id].transform.parent.transform.localEulerAngles;
        r.y += rotationDiffCheckpointPlayer;
        rb.MoveRotation(Quaternion.Euler(r));

        player.GetComponentInChildren<CarControl>().MovementReset();
    }

    public void ResetLoops()
    {
        
    }

    public string GetParentTag(GameObject obj)
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
        TimerManager.instance.StartTimer();
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
        checkpoint.SetNextIDs(new int[3] {1, 0, 0});
        checkpoints[0] = checkpoint;
    }

    public void OnPlayerDied()
    {
        StartCoroutine(ResetPlayer());
        UpdateLives();
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(2);
        Health playerHealth = player.GetComponent<Health>();
        if (isGameover)
        {
            TransferToCheckpoint(1, startPoint.transform.position);
        }
        else 
        {
            ToLastCheckpoint();
        }
        playerRespawned.RaiseEvent();
        player.GetComponent<CarSetup>().OnRestart();
        playerHealth.ResetHealth();
    }

    public void ResetLives()
    {
        currentLives = initialLives;
    }

    void UpdateLives()
    {
        currentLives = currentLives > 0 ? currentLives - 1 : 0;
        if (currentLives == 0)
        {
            gameover.RaiseEvent();
            isGameover = true;
        }
    }

    public int GetCurrentLoop()
    {
        return currentLoop;
    }

    public int GetTotalLoops()
    {
        return totalLoops;
    }
}
