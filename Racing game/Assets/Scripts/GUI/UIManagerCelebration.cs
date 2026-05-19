using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerCelebration : MonoBehaviour
{
    [SerializeField] GameObject personalRecordContainer, globalLeaderboardScrollView;
    [SerializeField] GameObject globalLeaderboardViewportContainer;
    [SerializeField] GameObject scoreEntryItemPrefab;
    [SerializeField] Button personalTimesBtn, globalTimesBtn;
    bool personalTimesInitialized;
    bool globalTimesInitialized;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPersonalTimes();
    }

    public void ShowPersonalTimes()
    {
        globalLeaderboardScrollView.SetActive(false);
        personalRecordContainer.SetActive(true);

        SetButtonHighlight(globalTimesBtn, false);
        SetButtonHighlight(personalTimesBtn, true);

        // bool to not reload ui every time
        if (personalTimesInitialized) return;

        var gameManager = GameManager.instance;

        PlayerProfile currentProfile = gameManager.GetCurrentProfile();
        string lastPlayedScene = gameManager.GetLastPlayedSceneName();
        LevelData currentLevelData = currentProfile.levels.Find(l => l.levelName == lastPlayedScene);

          
        if (currentLevelData == null) return;
        int rank = 1;

        foreach (float time in currentLevelData.bestTimes)
        {
            GameObject entryController = Instantiate(scoreEntryItemPrefab, personalRecordContainer.transform);
            ScoreEntryController entry = entryController.GetComponent<ScoreEntryController>();

            entry.HidePlayerNameWrapper();
            entry.SetRankTxt(rank.ToString());
            entry.SetTimeTxt(Utilities.FormatTime(time));

            rank++;
        }

        personalTimesInitialized = true;
    }

    public void ShowGlobalTimes()
    {
        personalRecordContainer.SetActive(false);
        globalLeaderboardScrollView.SetActive(true);

        SetButtonHighlight(globalTimesBtn, true);
        SetButtonHighlight(personalTimesBtn, false);

        if (globalTimesInitialized) return;

        int rank = 1;

        string lastPlayedScene = GameManager.instance.GetLastPlayedSceneName();

        List<PlayerProfile> allProfiles = GameManager.instance.GetAllProfiles();

        var sortedLeaderboard = allProfiles
            .Select(profile => new
            {
                ProfileName = profile.profileName,
                LevelInfo = profile.levels.Find(l => l.levelName == lastPlayedScene)
            })
            .Where(x => x.LevelInfo != null && x.LevelInfo.bestTimes.Count > 0)
            .OrderBy(x => x.LevelInfo.bestTimes[0])//in index 0 is the best time due to sorting in AddTime
            .ToList();

        foreach (var entry in sortedLeaderboard)
        {
            GameObject scoreEntryObject = Instantiate(scoreEntryItemPrefab, globalLeaderboardViewportContainer.transform);

            ScoreEntryController entryController = scoreEntryObject.GetComponent<ScoreEntryController>();

            entryController.SetPlayerName(entry.ProfileName);
            entryController.SetRankTxt(rank.ToString());

            float bestProfileTime = entry.LevelInfo.bestTimes[0];
            entryController.SetTimeTxt(Utilities.FormatTime(bestProfileTime));

            rank++;
        }

        globalTimesInitialized = true;
    }

    public void SetButtonHighlight(Button targetButton, bool isSelected)
    {
        ColorBlock cb = targetButton.colors;

        if (isSelected)
        {
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
        }
        else
        {
            cb.normalColor = Color.gray;
            cb.selectedColor = Color.gray;
        }

        targetButton.colors = cb;
    }
}
