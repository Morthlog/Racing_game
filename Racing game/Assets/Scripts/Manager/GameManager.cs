using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<string, SortedSet<float> > playerProfiles=new();
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
        GenerateMockData();
    }

    public bool IsProfileNameValid(string profileName)
    {
        string trimmed =profileName.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmed) || playerProfiles.ContainsKey(trimmed))
        {
            return false;
        }
        return true;
    }

    public void AddProfile(string profileName)
    {
        currentProfileName = profileName;
        currentProfileTimes = new SortedSet<float>();
        playerProfiles.Add(profileName, currentProfileTimes);
    }

    public void AddTime(float time)
    {
        currentProfileTimes.Add(time);
        if (currentProfileTimes.Count > 10)
        {
            currentProfileTimes.Remove(currentProfileTimes.Max);
        }
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
}
