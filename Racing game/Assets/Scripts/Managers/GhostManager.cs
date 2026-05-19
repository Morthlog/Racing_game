using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GhostManager : MonoBehaviour
{

    private GhostDataWrapper bestRecording;
    private GhostDataWrapper recording;
    private Dictionary<string, GhostDataWrapper> allData;
    private GameObject ghost;
    Rigidbody rb;
    private bool ghostExists = false;

    private bool iRecording = false;
    private int currentFrame = 0;
    private bool playingSaved = false;

    private GhostData previousFrame;
    private GhostData nextFrame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadData();
        string profileName = GameManager.instance.GetCurrentProfile().profileName;
        string level = SceneManager.GetActiveScene().name;
        GameObject car = MainGameSceneManager.instance.player.GetComponent<CarSetup>().playerData.chosenCar;
        string carName = car.name;

        recording = new GhostDataWrapper(profileName, level, carName, new List<GhostData>());
        GhostDataWrapper ghostDataWrapper;

        if (allData.TryGetValue(getKeyOf(recording), out ghostDataWrapper))
        {
            bestRecording = ghostDataWrapper;
            ghostExists = true;

            initGhost(ghostDataWrapper.carName);
        }

    }

    void FixedUpdate()
    {
        if (iRecording)
        {
            recordFrame();
        }
        if (playingSaved && ghostExists)
        {
            playNextTimedFrame(TimerManager.instance.GetTime());
        }
    }

    public void startRecording()
    {
        //Debug.Log("Start recording");
        iRecording = true;
        playingSaved = true;
        recording.ghostData.Clear();
    }

    public void stopRecording()
    {
        iRecording = false;
        playingSaved = false;
        saveRecording();
    }

    private void recordFrame()
    {
        GhostData frameData = new GhostData();
        Transform tr = MainGameSceneManager.instance.player.transform;
        frameData.position = new SerializableVector3(tr.position);
        frameData.rotation = new SerializableQuaternion(tr.rotation);
        frameData.time = TimerManager.instance.GetTime();
        recording.ghostData.Add(frameData);
    }

    private void playNextTimedFrame(float currTime)
    {
        if (previousFrame is null)
        {
            previousFrame = bestRecording.ghostData[currentFrame];
            nextFrame = bestRecording.ghostData[++currentFrame];
        }
        int frames = bestRecording.ghostData.Count - 1;
        while (nextFrame.time < currTime && currentFrame < frames)
        {
            currentFrame++;
            previousFrame = nextFrame;
            nextFrame = bestRecording.ghostData[currentFrame];
        }
        float interpolation = Mathf.InverseLerp(previousFrame.time, nextFrame.time, currTime);
        Vector3 position = Vector3.Lerp(
            previousFrame.position.toVector3(), nextFrame.position.toVector3(), interpolation);
        Quaternion rotation = Quaternion.Slerp(
            previousFrame.rotation.toQuaternion(), nextFrame.rotation.toQuaternion(), interpolation);

        rb.MovePosition(position);
        rb.MoveRotation(rotation);
    }

    private void saveRecording()
    {
        Debug.Log("saving");
        string key = getKeyOf(recording);
        float recordingTime = recording.ghostData.Last().time;
        if (!ghostExists ||
            allData[key].ghostData.Last().time > recordingTime)
        {
            allData[key] = recording;
        }

        string json = JsonConvert.SerializeObject(allData, Formatting.Indented);

        string path = Path.Combine(Application.persistentDataPath, "bestTime.json");
        File.WriteAllText(path, json);
    }


    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "bestTime.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            allData = JsonConvert.DeserializeObject<Dictionary<string, GhostDataWrapper>>(json);
        }
        if (allData is null)
        {
            allData = new Dictionary<string, GhostDataWrapper>();
        }
    }

    private string getKeyOf(GhostDataWrapper ghostDataWrapper)
    {
        return ghostDataWrapper.level;
    }

    private void initGhost(string carName)
    {
        GameObject prefab = Resources.Load<GameObject>("Cars/" + carName);
        ghost = Instantiate(prefab);
        rb = ghost.AddComponent<Rigidbody>();
        rb.useGravity = false;
        MeshCollider[] meshColliders = ghost.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider meshCollider in meshColliders)
        {
            meshCollider.enabled = false;
        }

        MeshRenderer[] meshes = ghost.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            Material mat = mesh.material;

            mat.shader = Shader.Find("Universal Render Pipeline/Unlit");

            mat.SetFloat("_Surface", 1f); // 1 = Transparent

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);

            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = 3000;
            Color c = mat.color;
            c.a = 0.5f;
            mat.color = c;
        }
    }
}
