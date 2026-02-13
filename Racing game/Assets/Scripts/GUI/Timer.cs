using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    float currentTime = 0;
    float lapTime = 0;
    bool playTimer = false;
    Color ogColor;
    Color highlightColor;
    float highlightTimeLength = 2;
    float highlightTimer = 0;
    bool highlighting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        ogColor = textMeshProUGUI.color;
        highlightColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playTimer) return;

        highlightTimer += Time.deltaTime;
        currentTime += Time.deltaTime;
        lapTime += Time.deltaTime;
        if (highlighting && highlightTimer < highlightTimeLength) return;
        if (highlighting) StopHighlight();


        DrawTime(currentTime);

    }

    public void StartTimer()
    {
        playTimer = true;
    }

    public void StopTimer()
    {
        playTimer = false;
    }

    public void HighlightTimer()
    {
        textMeshProUGUI.color = highlightColor;
        highlightTimer = 0;
        highlighting = true;
        DrawTime(lapTime);
    }

    public void StopHighlight()
    {
        highlighting = false;
        textMeshProUGUI.color = ogColor;
        lapTime = 0;
    }

    private void DrawTime(float currentTime)
    {  
        textMeshProUGUI.text = Utilities.FormatTime(currentTime);
    }
    public float GetTime()
    {
        return currentTime;
    }
}
