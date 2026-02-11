using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    Animator animator;
    string[] texts;
    int textIndex = 0;
    [Header("Events")]
    [SerializeField] private VoidEventChannelSO countdownCounting;
    [SerializeField] private VoidEventChannelSO countdownDone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        texts = new string[] {
                "3",
                "2",
                "1",
                "Go"
        };
    }


    public void StartCountdown()
    {
        animator.SetBool("Play", true);
    }

    public void NextText()
    {
        textMeshProUGUI.text = texts[textIndex];
        if (textIndex == texts.Length - 1)
        {
            CountdownManager.instance.AllowMovement();
            countdownDone.RaiseEvent();
        }
        else
        {
            countdownCounting.RaiseEvent();
        }
            
    }

    public void IDone()
    {
        if (++textIndex >= texts.Length)
            Disable();
    }

    private void Disable()
    {
        animator.SetBool("Play", false);
        gameObject.SetActive(false);
    }
}
