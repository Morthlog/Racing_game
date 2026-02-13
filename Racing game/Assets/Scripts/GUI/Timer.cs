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


        drawTime(currentTime);

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
        drawTime(lapTime);
    }

    public void StopHighlight()
    {
        highlighting = false;
        textMeshProUGUI.color = ogColor;
        lapTime = 0;
    }

    private void drawTime(float currentTime)
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        string formatted = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        textMeshProUGUI.text = formatted;
    }

    public float GetTime()
    {
        return currentTime;
    }
}
