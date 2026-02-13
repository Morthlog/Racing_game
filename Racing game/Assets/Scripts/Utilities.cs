using UnityEngine;

public static class Utilities
{
    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        string formatted = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        return formatted;
    }
}
