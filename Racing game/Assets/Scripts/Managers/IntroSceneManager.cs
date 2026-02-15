using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] GameObject menuCamvas, choiceCanvas;
    [SerializeField] GameObject nextBtn, previousBtn;
    [SerializeField] GameObject errorWrapper;
    [SerializeField] GameObject profileCreationPanel;
    [SerializeField] GameObject profileSelectionPanel;
    [SerializeField] TMP_InputField profileInputField;
    [SerializeField] TextMeshProUGUI carNameTxt;


    [SerializeField] GameObject loadedProfileBtnPrefab;
    [SerializeField] GameObject viewportContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCamvas.SetActive(true);
        choiceCanvas.SetActive(false);
    }

    void GoToCarSelection()
    {
        menuCamvas.SetActive(false);
        profileSelectionPanel.SetActive(false);
        choiceCanvas.SetActive(true);      
    }

    public void SetButtons(int carIntex, int carPoolLength)
    {
        if (carIntex + 1 == carPoolLength)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }


        if (carIntex - 1 < 0)
        {
            previousBtn.SetActive(false);
        }
        else
        {
            previousBtn.SetActive(true);
        }
    }

    public void SetCarNameTxt(string name)
    {
        carNameTxt.text = name;
    }

    public void OnInputChange()
    {
        ValidateNameAndShowError();
    }
    bool ValidateNameAndShowError()
    {
        string input = profileInputField.text;

        if (!GameManager.instance.IsProfileNameValid(input))
        {
            errorWrapper.SetActive(true);
            return false;
        }
    
        errorWrapper.SetActive(false);
        return true;
    }

    public void CreateProfile()
    {
        if (!ValidateNameAndShowError()) return;

        GameManager.instance.AddProfile(profileInputField.text);
        GoToCarSelection();
    }

    public void EnableProfileCreationPanel()
    {
        profileCreationPanel.SetActive(true);
    }

    public void EnableProfileSelectionPanel()
    {
        profileSelectionPanel.SetActive(true);
    }

    public void PopulateProfileList()
    {
        foreach (var (profileName, times) in GameManager.instance.GetAllProfiles())
        {
            GameObject profileBtnWrapper = Instantiate(loadedProfileBtnPrefab, viewportContainer.transform);

            Button currentBtn = profileBtnWrapper.GetComponent<Button>();
            profileBtnWrapper.name = profileName;
            currentBtn.GetComponentInChildren<TextMeshProUGUI>().text = profileName;
            currentBtn.onClick.AddListener(() => GameManager.instance.SetCurrentProfileName(profileName));
            currentBtn.onClick.AddListener(() => GameManager.instance.SetCurrentProfileTimes(times));
            currentBtn.onClick.AddListener(() => GoToCarSelection());
        }
    }
}
