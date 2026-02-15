using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] BoolEventChannelSO gamePaused;
    [SerializeField] VoidEventChannelSO gameOver;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject diedText;
    bool isGameOver = false;

    void SetPauseMenuVisibility(bool visible)
    {
        if (isGameOver) return;
        pauseMenu.SetActive(visible);
        diedText.SetActive(false);
    }

    void ShowDiedMenu()
    {
        isGameOver = true;
        pauseMenu.SetActive(true);
        diedText.SetActive(true);
    }
    private void OnEnable()
    {
        gameOver.OnEventRaised += ShowDiedMenu;
        gamePaused.OnEventRaised += SetPauseMenuVisibility;
    }


    private void OnDisable()
    {
        gameOver.OnEventRaised -= ShowDiedMenu;
        gamePaused.OnEventRaised -= SetPauseMenuVisibility;
    }
}
