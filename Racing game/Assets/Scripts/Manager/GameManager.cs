using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Dictionary<string, float> playerProfiles=new();
    public static GameManager instance;

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

    public bool IsProfileNameValid(string profileName)
    {
        string trimmed =profileName.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmed) || playerProfiles.ContainsKey(trimmed))
        {
            return false;
        }
        return true;
    }

    public void AddProfile(string profile)
    {

        playerProfiles.Add(profile, 0);
    }
}
