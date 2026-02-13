using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<string, SortedSet<float>> playerProfiles = new();
    public static GameManager instance;

    public string currentProfileName;
    public SortedSet<float> currentProfileTimes;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadData();
        GenerateMockData();
    }

    public bool IsProfileNameValid(string profileName)
    {
        string trimmed = profileName.Trim();

        if (string.IsNullOrWhiteSpace(trimmed) || playerProfiles.ContainsKey(trimmed))
        {
            return false;
        }
        return true;
    }

    public void AddProfile(string profileName)
    {
        SetCurrentProfileName(profileName);
        SetCurrentProfileTimes(new SortedSet<float>());
        playerProfiles.TryAdd(profileName, currentProfileTimes);
        SaveData();
    }

    public void SetCurrentProfileName(string profileName)
    {
        currentProfileName = profileName;
    }

    public void SetCurrentProfileTimes(SortedSet<float> times)
    {
        currentProfileTimes = times;
    }

    public void AddTime(float time)
    {
        currentProfileTimes.Add(time);
        if (currentProfileTimes.Count > 10)
        {
            currentProfileTimes.Remove(currentProfileTimes.Max);
        }
        SaveData();
    }

    public void GenerateMockData()
    {
        for (int i = 1; i <= 5; i++)
        {
            string name = "Player_" + i;

            AddProfile(name);

            for (int j = 0; j < 5; j++)
            {
                float randomTime = Random.Range(10.0f, 100.0f);

                randomTime = Mathf.Round(randomTime * 100f) / 100f;

                AddTime(randomTime);
            }
        }
    }

    public Dictionary<string, SortedSet<float>> GetAllProfiles()
    {
        var copy = new Dictionary<string, SortedSet<float>>();

        foreach (var entry in playerProfiles)
        {
            copy.Add(entry.Key, new SortedSet<float>(entry.Value));
        }

        return copy;
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(playerProfiles, Formatting.Indented);

        string path = Path.Combine(Application.persistentDataPath, "profiles.json");
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "profiles.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            playerProfiles = JsonConvert.DeserializeObject<Dictionary<string, SortedSet<float>>>(json);
        }
    }
}
