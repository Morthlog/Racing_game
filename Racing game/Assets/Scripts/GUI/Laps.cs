using TMPro;
using UnityEngine;

public class Laps : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    private int count;
    private int total;

    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        total = MainGameSceneManager.instance.GetTotalLoops();
        count = MainGameSceneManager.instance.GetCurrentLoop();
        string text = "Lap " + count + "/" + total;
        m_TextMeshProUGUI.text = text;
    }

    public void NextLap()
    {
        count = MainGameSceneManager.instance.GetCurrentLoop();
        string text = "Lap " + count + "/" + total;
        m_TextMeshProUGUI.text = text;
    }
}
