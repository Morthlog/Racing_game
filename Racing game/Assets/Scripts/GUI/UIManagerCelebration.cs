using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerCelebration : MonoBehaviour
{
    [SerializeField] GameObject personalRecordContainer, globalLeaderboardContainer;

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
        globalLeaderboardContainer.SetActive(false);
        personalRecordContainer.SetActive(true);

        SetButtonHighlight(globalTimesBtn, false);
        SetButtonHighlight(personalTimesBtn, true);

        if (personalTimesInitialized) return;

        int rank = 1;
        foreach (float time in GameManager.instance.currentProfileTimes)
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
        globalLeaderboardContainer.SetActive(true);

        SetButtonHighlight(globalTimesBtn, true);
        SetButtonHighlight(personalTimesBtn, false);

        if (globalTimesInitialized) return;
        int rank = 1;
        Dictionary<string, SortedSet<float>> profiles = GameManager.instance.GetAllProfiles();
        foreach (var (profileName, times) in profiles)
        {
            GameObject scoreEntryObject = Instantiate(scoreEntryItemPrefab, globalLeaderboardContainer.transform);

            ScoreEntryController entryController = scoreEntryObject.GetComponent<ScoreEntryController>();
            entryController.SetPlayerName(profileName);
            entryController.SetRankTxt(rank.ToString());
            float bestProfileTime = times.Min;
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

    public void ReturnToIntroScene()
    {
        SceneManager.LoadSceneAsync("Intro");
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync("Main Game");
    }
}
