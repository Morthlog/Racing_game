using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCamvas, choiceCanvas;
    [SerializeField]
    GameObject nextBtn, previousBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCamvas.SetActive(true);
        choiceCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        menuCamvas.SetActive(false);
        choiceCanvas.SetActive(true);
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene("SampleScene");
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
}
