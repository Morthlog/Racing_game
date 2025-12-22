using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCamvas, choiceCanvas;
    [SerializeField]
    GameObject nextBtn, previousBtn;

    [SerializeField]
    TextMeshProUGUI carNameTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCamvas.SetActive(true);
        choiceCanvas.SetActive(false);
    }

    public void NewGame()
    {
        menuCamvas.SetActive(false);
        choiceCanvas.SetActive(true);
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene("Main Game");
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
}
