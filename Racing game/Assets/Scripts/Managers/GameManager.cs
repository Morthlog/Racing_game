using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveDataList gameData = new SaveDataList();
    public static GameManager instance;

    private PlayerProfile currentProfile;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO newGame;
    [SerializeField] VoidEventChannelSO toMainGame;
    [SerializeField] VoidEventChannelSO restartLevel;
    [SerializeField] VoidEventChannelSO quitGame;
    [SerializeField] BoolEventChannelSO gamePaused;

    private GameInputActions actions;

    private bool isGamePaused = false;
    private bool isGameOver = false;
    private string lastPlayedSceneName;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        actions = new GameInputActions();

        //string path = Path.Combine(Application.persistentDataPath, "profiles.json");
        //if (File.Exists(path))
        //{
        //    File.Delete(path);
        //}

        LoadData();
        GenerateMockData();
    }

    
    public void SetLastPlayedceneName(string currentSceneName)
    {
        this.lastPlayedSceneName = currentSceneName;
    }

    public string GetLastPlayedSceneName()
    {
        return lastPlayedSceneName;
    }

    public bool IsProfileNameValid(string profileName)
    {
        string trimmed = profileName.Trim();

        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return false;
        }

        return !gameData.profiles.Exists(p => p.profileName.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
    }

    public void AddProfile(string profileName)
    {
        string trimmed = profileName.Trim();
        if (!IsProfileNameValid(trimmed)) return;

        PlayerProfile newProfile = new PlayerProfile { profileName = trimmed };
        gameData.profiles.Add(newProfile);

        currentProfile = newProfile;

        SaveData();
    }

    public void SetCurrentProfileByName(string profileName)
    {
        PlayerProfile profile = gameData.profiles.Find(p => p.profileName.Equals(profileName, StringComparison.OrdinalIgnoreCase));
        if (profile == null) return;
        
        currentProfile = profile;
    }

    public void AddTime(string levelName, float time)
    {
        LevelData level = currentProfile.levels.Find(l => l.levelName == levelName);

        if (level == null)
        {
            level = new LevelData { levelName = levelName };
            currentProfile.levels.Add(level);
        }

        level.bestTimes.Add(time);
        level.bestTimes.Sort();

        if (level.bestTimes.Count > 10)
        {
            level.bestTimes.RemoveAt(level.bestTimes.Count - 1);//removing highest time
        }

        SaveData();
    }

    public void GenerateMockData()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;   
   
        // starting index from 1 to skip the menu sceen 
        for (int sceneIndex = 1; sceneIndex < sceneCount; sceneIndex++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            for (int i = 0; i < 2; i++)
            {
                string name = "Mock_Player_" + i;
                AddProfile(name);
                float randomTime = UnityEngine.Random.Range(10.0f, 100.0f);
                randomTime = Mathf.Round(randomTime * 100f) / 100f;

                AddTime(sceneName, randomTime);
            }
        }
    }

    public List<PlayerProfile> GetAllProfiles()
    {
        return gameData.profiles;
    }

    public PlayerProfile GetCurrentProfile()
    {
        return currentProfile;
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, "profiles.json");
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "profiles.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameData = JsonConvert.DeserializeObject<SaveDataList>(json);
            
            // Set a current profile in case we play directly a scene for debug and not through intro scene
            if (gameData.profiles.Count > 0)
            {
                currentProfile = gameData.profiles[0];
            }
        }
    }

    private void ChangeLevelToMain()
    {
        LoadLevelByName("Main Game");
        SetGameOver(false);
    }

    private void ChangeLevelToIntro()
    {
        LoadLevelByName("Intro");
        SetGameOver(false);
    }

    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
        UnFreezeGame();
    }

    public void LoadLevelByIndex(int idx)
    {
        SceneManager.LoadSceneAsync(idx);
        UnFreezeGame();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    public void UnFreezeGame()
    {
        Time.timeScale = 1f;
    }

    public void SetGameOver(bool value)
    {
        isGameOver = value;
        if (isGameOver)
        {
            FreezeGame();
        }
    }

    private void HandlePauseGame(InputAction.CallbackContext context)
    {
        if (isGameOver) return;

        isGamePaused = !isGamePaused;
        gamePaused.RaiseEvent(isGamePaused);

        if (isGamePaused)
            FreezeGame();
        else
            UnFreezeGame();
    }

    private void RestartLevel()
    {
        LoadLevelByName(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        newGame.OnEventRaised += ChangeLevelToIntro;
        restartLevel.OnEventRaised += RestartLevel;
        toMainGame.OnEventRaised += ChangeLevelToMain;
        quitGame.OnEventRaised += Application.Quit;

        actions.Menu.Enable();
        actions.Menu.Pause.performed += HandlePauseGame;
    }

    private void OnDisable()
    {
        if (instance != this) return;//used to prevent errors when object is destroyed
        newGame.OnEventRaised -= ChangeLevelToIntro;
        restartLevel.OnEventRaised -= RestartLevel;
        toMainGame.OnEventRaised -= ChangeLevelToMain;
        quitGame.OnEventRaised -= Application.Quit;

        actions.Menu.Pause.performed -= HandlePauseGame;
        actions.Menu.Disable();
    }
}

[Serializable]
public class LevelData
{
    public string levelName;
    public List<float> bestTimes = new List<float>();
}

[Serializable]
public class PlayerProfile
{
    public string profileName;
    public List<LevelData> levels = new List<LevelData>();
}

[Serializable]
public class SaveDataList
{
    public List<PlayerProfile> profiles = new List<PlayerProfile>();
}