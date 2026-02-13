using UnityEngine;
using TMPro;

public class ScoreEntryController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankTxt, playerNameTxt, timeTxt;
    [SerializeField] GameObject playerNameWrapper;
    public void SetRankTxt(string value)
    {
        rankTxt.text = value;
    }

    public void SetPlayerName(string value)
    {
        playerNameTxt.text = value;
    }

    public void SetTimeTxt(string value)
    {
        timeTxt.text = value;
    }

    public void HidePlayerNameWrapper()
    {
        playerNameWrapper.SetActive(false);
    }

    public void ShowPlayerNameWrapper()
    {
        playerNameWrapper.SetActive(true);
    }
}
