using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] GameObject menuCamvas, choiceCanvas;
    [SerializeField] GameObject nextBtn, previousBtn;
    [SerializeField] GameObject errorWrapper;
    [SerializeField] GameObject profilePanel;
    [SerializeField] TMP_InputField profileInputField;
    [SerializeField] TextMeshProUGUI carNameTxt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCamvas.SetActive(true);
        choiceCanvas.SetActive(false);
    }

    void EnableNewGamePanel()
    {
        menuCamvas.SetActive(false);
        choiceCanvas.SetActive(true);
    }

    public void ChangeLevel()
    {
        SceneManager.LoadSceneAsync("Main Game");
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
        EnableNewGamePanel();
    }

    public void EnableProfilePanel()
    {
        profilePanel.SetActive(true);
    }
}
